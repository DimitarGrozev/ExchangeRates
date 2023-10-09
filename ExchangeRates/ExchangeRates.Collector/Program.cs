using ExchangeRates.Configuration;
using ExchangeRates.Data;
using ExchangeRates.Services;
using Fixerr.Installer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
        .ConfigureServices((context, services) =>
        {
            Configure(services);
        })
    .Build();

var dbContext = host.Services.GetService<ExchangeRatesDbContext>();

if(dbContext == null)
{
    return;
}

await dbContext.Database.EnsureCreatedAsync();
await dbContext.Database.MigrateAsync();

host.Run();


void Configure(IServiceCollection services)
{
    var configRoot = BuilderConfiguration();

    services.AddFixer(configRoot.GetValue<string>("Fixer_API_Key"));
    services.AddSingleton(ConnectionMultiplexer.Connect(configRoot.GetConnectionString("Redis")));
    services.AddScoped<MessageClientService>();
    services.AddDbContext<ExchangeRatesDbContext>(
                options => SqlServerDbContextOptionsExtensions.UseSqlServer(options, configRoot.GetConnectionString("Database")));
    services.Configure<ExchangeRatesConfiguration>(configRoot.GetSection("ExchangeRatesConfiguration"));
    services.Configure<MessageClientConfiguration>(configRoot.GetSection("MessageClientConfiguration"));
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