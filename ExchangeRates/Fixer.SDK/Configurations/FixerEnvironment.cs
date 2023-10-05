// ---------------------------------------------------------------
// Copyright (c) Pritom Purkayasta All rights reserved.
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

namespace Fixerr.Configurations;
internal static class FixerEnvironment
{
    internal const string HttpClientName = "Fixerr";
    private const string BaseUrl = "data.fixer.io/api/";
    internal static Uri BaseUri => IsPaidSubscription ? new Uri($"https://{BaseUrl}") : new Uri($"http://{BaseUrl}");
    internal static string FixerDateFormat { get; } = "yyyy-MM-dd";

    private static string _apiKey = string.Empty;

    /// <summary>
    /// This is where you should set your fixer's api key.
    /// </summary>
    internal static string ApiKey
    {
        set => _apiKey = value;
        get
        {
            if (string.IsNullOrEmpty(_apiKey) || string.IsNullOrWhiteSpace(_apiKey))
                throw new NullReferenceException($"{nameof(ApiKey)} is empty! Please Set the appropriate value");
            return _apiKey;
        }
    }

    /// <summary>
    /// If you have purchased a non free licence then you should set it to <code>true</code>
    /// <para>This is important because of SSL Certification. </para>
    /// <para>If you are using the free licence then it will use the http protocol, if not it will use https</para>
    /// </summary>
    /// <returns>a boolean value indicating the status</returns>
    internal static bool IsPaidSubscription { get; set; } = false;
}
