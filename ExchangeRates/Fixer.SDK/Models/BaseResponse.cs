// ---------------------------------------------------------------
// Copyright (c) Pritom Purkayasta All rights reserved.
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Fixerr.Models;

public abstract class BaseResponse
{
    [JsonPropertyName("success")] public bool Success { get; set; }

    [JsonPropertyName("error")] public ErrorInfo? ErrorInfo { get; set; }
}

public class ErrorInfo
{
    [JsonPropertyName("code")] public int Code { get; set; }

    [JsonPropertyName("type")] public string? Type { get; set; }
}