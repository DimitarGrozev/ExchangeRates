// ---------------------------------------------------------------
// Copyright (c) Pritom Purkayasta All rights reserved.
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Fixerr.Models;

namespace Fixerr;

public partial interface IFixerClient
{
    /// <summary>
    /// returns the converted currency from one to another
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="amount"></param>
    /// <param name="historicalDate"></param>
    /// <param name="apiKey"></param>
    /// <returns></returns>
    Task<CurrencyConverter?> GetCurrencyConverterAsync(
        string from,
        string to,
        int amount,
        string? historicalDate = null,
        string? apiKey = null);

    /// <summary>
    /// returns the converted currency from one to another
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="amount"></param>
    /// <param name="historicalDate"></param>
    /// <param name="apiKey"></param>
    /// <returns></returns>
    Task<HttpResponseMessage> GetCurrencyConverterRawAsync(
        string from,
        string to,
        int amount,
        string? historicalDate = null,
        string? apiKey = null);

    /// <summary>
    /// returns the converted currency from one to another
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="amount"></param>
    /// <param name="historicalDate"></param>
    /// <param name="apiKey"></param>
    /// <returns></returns>
    Task<string> GetCurrencyConverterStringAsync(
        string from,
        string to,
        int amount,
        string? historicalDate = null,
        string? apiKey = null);
}