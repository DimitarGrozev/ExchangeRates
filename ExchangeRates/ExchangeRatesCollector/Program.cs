using Fixerr.Installer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
        .ConfigureServices((context, services) =>
        {
            Configure(services);
        })
    .Build();

host.Run();


void Configure(IServiceCollection services)
{
    var configRoot = BuilderConfiguration();
    services.AddFixer(configRoot.GetValue<string>("Fixer_API_Key"));
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