using Fixerr.Configurations;
using Microsoft.Extensions.DependencyInjection;

namespace Fixerr.Installer;

public static class FixerExtensions
{
    #region Microsoft DI

    /// <summary>
    /// Add fixer using apikey and subscription flag for https
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <param name="apiKey"></param>
    /// <param name="isPaidSubscription">If you have purchased a non free license then you should set it to <code>true</code>,
    /// By Default it is false, This is important because of SSL Certification. If you are using the free license then it will use the
    /// http protocol, if not it will use https</param>
    /// <exception cref="NullReferenceException"></exception>
    public static void AddFixer(
        this IServiceCollection serviceCollection,
        string? apiKey,
        bool isPaidSubscription = false)
    {
        ArgumentException.ThrowIfNullOrEmpty(apiKey, nameof(apiKey));

        FixerEnvironment.ApiKey = apiKey;
        FixerEnvironment.IsPaidSubscription = isPaidSubscription;

        serviceCollection.AddScoped<IFixerClient, FixerClient>();
    }

    #endregion
}