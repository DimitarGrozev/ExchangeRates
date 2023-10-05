// ---------------------------------------------------------------
// Copyright (c) Pritom Purkayasta All rights reserved.
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Fixerr.Models;

public sealed class LatestRate : BaseResponse
{
    /// <summary>
    /// Returns the exact date and time (UNIX time stamp) the given rates were collected.
    /// </summary>
    [JsonPropertyName("timestamp")]
    public int? TimeStamp { get; set; }

    /// <summary>
    /// Returns the three-letter currency code of the base currency used for this request.
    /// </summary>

    [JsonPropertyName("base")]
    public string? Base { get; set; }

    [JsonPropertyName("date")] public DateTime? Date { get; set; }

    /// <summary>
    /// Returns exchange rate data for the currencies you have requested.
    /// </summary>

    [JsonPropertyName("rates")]
    public Dictionary<string, double>? Rates { get; set; }
}