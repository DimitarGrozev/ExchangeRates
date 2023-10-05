// ---------------------------------------------------------------
// Copyright (c) Pritom Purkayasta All rights reserved.
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Fixerr.Models;

public sealed class CurrencyConverter : BaseResponse
{
    [JsonPropertyName("query")] public QueryInfo? Query { get; set; }

    [JsonPropertyName("info")] public Info? Info { get; set; }

    [JsonPropertyName("historical")] public bool IsHistorical { get; set; }

    [JsonPropertyName("date")] public DateTime? Date { get; set; }

    [JsonPropertyName("result")] public double ConvertionResult { get; set; }
}

public sealed class QueryInfo
{
    [JsonPropertyName("from")] public string? Source { get; set; }

    [JsonPropertyName("to")] public string? Destination { get; set; }

    [JsonPropertyName("amount")] public int Amount { get; set; }
}

public sealed class Info
{
    [JsonPropertyName("timestamp")] public int TimeStamp { get; set; }

    [JsonPropertyName("rate")] public double Rate { get; set; }
}