// ---------------------------------------------------------------
// Copyright (c) Pritom Purkayasta All rights reserved.
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Fixerr.Models;

namespace Fixerr;

public partial interface IFixerClient
{
    /// <summary>
    /// returns daily historic rates between two dates
    /// </summary>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <param name="baseCurrency"></param>
    /// <param name="symbols"></param>
    /// <param name="apiKey"></param>
    /// <returns></returns>
    Task<TimeSeries?> GetTimeSeriesAsync(
        string startDate,
        string endDate,
        string? baseCurrency = null,
        string? symbols = null,
        string? apiKey = null);

    /// <summary>
    /// returns daily historic rates between two dates
    /// </summary>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <param name="baseCurrency"></param>
    /// <param name="symbols"></param>
    /// <param name="apiKey"></param>
    /// <returns></returns>
    Task<HttpResponseMessage> GetTimeSeriesRawAsync(
        string startDate,
        string endDate,
        string? baseCurrency = null,
        string? symbols = null,
        string? apiKey = null);

    /// <summary>
    /// returns daily historic rates between two dates
    /// </summary>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <param name="baseCurrency"></param>
    /// <param name="symbols"></param>
    /// <param name="apiKey"></param>
    /// <returns></returns>
    Task<string> GetTimeSeriesStringAsync(
        string startDate,
        string endDate,
        string? baseCurrency = null,
        string? symbols = null,
        string? apiKey = null);
}