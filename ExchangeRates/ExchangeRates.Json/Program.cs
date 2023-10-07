using ExchangeRates.Configuration;
using ExchangeRates.Data;
using ExchangeRates.Json.Utilities.Middlewares;
using ExchangeRates.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using System.Net;
using ThrottlingTroll;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults((context, builder) =>
    {
        builder
        .UseMiddleware<ClientRegisteredMiddleware>()
        .UseThrottlingTroll(context, options =>
        {
            options.Config = new ThrottlingTrollConfig
            {
                Rules = new[]
                {
                    new ThrottlingTrollRule
                    {
                        UriPattern = "api/currentExchangeRate",
                        HeaderValue = "60",
                        Method = "POST",
                        LimitMethod = new FixedWindowRateLimitMethod
                        {
                            PermitLimit = 1,
                            IntervalInSeconds = 60
                        },
                        IdentityIdExtractor = (request) =>
                        {
                            return ((IIncomingHttpRequestProxy)request).Request.Query["clientId"];
                        }
                    }
                }
            };
            options.ResponseFabric = async (limitExceededResult, requestProxy, responseProxy, requestAborted) =>
            {
                responseProxy.StatusCode = (int)HttpStatusCode.TooManyRequests;

                responseProxy.SetHttpHeader("Retry-After", limitExceededResult.RetryAfterHeaderValue);

                await responseProxy.WriteAsync("Too many requests. Try again later.");
            };
        });
    })
    .ConfigureServices((context, services) =>
    {
        Configure(services);
    })
    .Build();


var dbContext = host.Services.GetService<ExchangeRatesDbContext>();

if (dbContext == null)
{
    return;
}

await dbContext.Database.EnsureCreatedAsync();
await dbContext.Database.MigrateAsync();

host.Run();


void Configure(IServiceCollection services)
{
    var configRoot = BuilderConfiguration();

    services.AddScoped<RequestValidationService>();
    services.AddScoped<StatisticsCollectorService>();
    services.AddSingleton(ConnectionMultiplexer.Connect(configRoot.GetConnectionString("Redis")));
    services.AddDbContext<ExchangeRatesDbContext>(
                options => SqlServerDbContextOptionsExtensions.UseSqlServer(options, configRoot.GetConnectionString("Database")));
    services.Configure<ExchangeRatesConfiguration>(configRoot.GetSection("ExchangeRatesConfiguration"));
    services.Configure<RegisteredUsersConfiguration>(configRoot.GetSection("RegisteredUsersConfiguration"));
}

IConfigurationRoot BuilderConfiguration()
{
    return new ConfigurationBuilder()
       .SetBasePath(Environment.CurrentDirectory)
       .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
       .AddEnvironmentVariables()
       .AddUserSecrets<Program>()
       .Build();
}