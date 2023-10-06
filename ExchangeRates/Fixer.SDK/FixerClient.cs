// ---------------------------------------------------------------
// Copyright (c) Pritom Purkayasta All rights reserved.
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Runtime.CompilerServices;
using Fixerr.Configurations;
using Microsoft.Extensions.Options;

[assembly: InternalsVisibleTo("FixerrTests")]

namespace Fixerr;

internal partial class FixerClient : IFixerClient
{
    private static readonly HttpClient? HttpClient = new HttpClient();

    static FixerClient()
    {
        HttpClient.BaseAddress = FixerEnvironment.BaseUri;
    }

    public FixerClient()
    {
    }

    public FixerClient(HttpClient? httpClient, IOptions<FixerOptions> options)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(options);

        FixerEnvironment.ApiKey = options.Value.ApiKey!;
        FixerEnvironment.IsPaidSubscription = options.Value.IsPaidSubscription;
    }
}