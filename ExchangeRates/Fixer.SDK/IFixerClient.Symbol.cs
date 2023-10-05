// ---------------------------------------------------------------
// Copyright (c) Pritom Purkayasta All rights reserved.
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Fixerr.Models;

namespace Fixerr;

public partial interface IFixerClient
{
    /// <summary>
    /// Returns all available currencies
    /// </summary>
    /// <returns>Symbol</returns>
    Task<Symbol?> GetSymbolAsync(string? apiKey = null);

    /// <summary>
    /// Returns all available currencies
    /// </summary>
    /// <returns>HttpResponseMessage</returns>
    Task<HttpResponseMessage> GetSymbolRawAsync(string? apiKey = null);

    /// <summary>
    /// Returns all available currencies
    /// </summary>
    /// <returns>string</returns>
    Task<string> GetSymbolStringAsync(string? apiKey = null);
}