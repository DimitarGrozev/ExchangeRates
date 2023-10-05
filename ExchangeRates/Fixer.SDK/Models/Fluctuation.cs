// ---------------------------------------------------------------
// Copyright (c) Pritom Purkayasta All rights reserved.
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Fixerr.Models;

public sealed class Fluctuation : BaseResponse
{
    [JsonPropertyName("start_date")] public DateTime? StartDate { get; set; }

    [JsonPropertyName("end_date")] public DateTime? EndDate { get; set; }

    [JsonPropertyName("base")] public string? Base { get; set; }

    [JsonPropertyName("rates")] public Dictionary<string, FluctuationRateInfo>? Rates { get; set; }
}

public sealed class FluctuationRateInfo
{
    [JsonPropertyName("start_rate")] public double StartRate { get; set; }

    [JsonPropertyName("end_rate")] public double EndRate { get; set; }

    [JsonPropertyName("change")] public double Change { get; set; }

    [JsonPropertyName("change_pct")] public double ChangePercentage { get; set; }
}