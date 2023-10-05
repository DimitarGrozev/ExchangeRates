// ---------------------------------------------------------------
// Copyright (c) Pritom Purkayasta All rights reserved.
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Fixerr.Models;

public sealed class HistoricRate : BaseResponse
{
    [JsonPropertyName("historical")] public bool Historical { get; set; }

    [JsonPropertyName("date")] public DateTime? Date { get; set; }

    [JsonPropertyName("timestamp")] public int? TimeStamp { get; set; }

    [JsonPropertyName("base")] public string? Base { get; set; }

    [JsonPropertyName("rates")] public Dictionary<string, string>? Rates { get; set; }
}