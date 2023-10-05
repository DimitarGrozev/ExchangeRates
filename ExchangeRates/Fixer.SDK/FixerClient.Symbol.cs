// ---------------------------------------------------------------
// Copyright (c) Pritom Purkayasta All rights reserved.
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------


using System.Text;
using System.Text.Json;
using Fixerr.Configurations;
using Fixerr.Models;

namespace Fixerr;

internal sealed partial class FixerClient : IFixerClient
{
    public async Task<Symbol?> GetSymbolAsync(string? apiKey = null)
    {
        string url = BuildSymbolUrl(apiKey);

        var streamResponse = await HttpClient!.GetStreamAsync(url);
        var symbolResponse = await JsonSerializer.DeserializeAsync<Symbol>(streamResponse).ConfigureAwait(false);
        return symbolResponse;
    }

    public Task<HttpResponseMessage> GetSymbolRawAsync(string? apiKey = null)
    {
        string url = BuildSymbolUrl(apiKey);
        return HttpClient!.GetAsync(url);
    }

    public Task<string> GetSymbolStringAsync(string? apiKey = null)
    {
        string url = BuildSymbolUrl(apiKey);
        return HttpClient!.GetStringAsync(url);
    }

    private static string BuildSymbolUrl(string? apiKey)
    {
        StringBuilder urlBuilder = new();
        urlBuilder.Append("symbols");
        urlBuilder.Append($"?access_key={apiKey ?? FixerEnvironment.ApiKey}");
        return urlBuilder.ToString();
    }
}