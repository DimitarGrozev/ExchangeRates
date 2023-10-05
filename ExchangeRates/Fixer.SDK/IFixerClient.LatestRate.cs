// ---------------------------------------------------------------
// Copyright (c) Pritom Purkayasta All rights reserved.
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Fixerr.Models;

namespace Fixerr;

public partial interface IFixerClient
{
    /// <summary>
    /// returns real time exchange data
    /// </summary>
    /// <param name="baseCurrency"></param>
    /// <param name="symbols"></param>
    /// <param name="apiKey"></param>
    /// <returns></returns>
    Task<LatestRate?> GetLatestRateAsync(
        string? baseCurrency = null,
        string? symbols = null,
        string? apiKey = null);

    /// <summary>
    /// returns real time exchange data
    /// </summary>
    /// <param name="baseCurrency"></param>
    /// <param name="symbols"></param>
    /// <param name="apiKey"></param>
    /// <returns></returns>
    Task<HttpResponseMessage> GetLatestRateRawAsync(
        string? baseCurrency = null,
        string? symbols = null,
        string? apiKey = null);

    /// <summary>
    /// returns real time exchange data
    /// </summary>
    /// <param name="baseCurrency"></param>
    /// <param name="symbols"></param>
    /// <param name="apiKey"></param>
    /// <returns></returns>
    Task<string> GetLatestRateStringAsync(
        string? baseCurrency = null,
        string? symbols = null,
        string? apiKey = null);
}

