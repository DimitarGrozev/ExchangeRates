// ---------------------------------------------------------------
// Copyright (c) Pritom Purkayasta All rights reserved.
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Fixerr.Models;

public sealed class Symbol : BaseResponse
{
    [JsonPropertyName("symbols")]
    public Dictionary<string, string>? Symbols { get; set; }
}
