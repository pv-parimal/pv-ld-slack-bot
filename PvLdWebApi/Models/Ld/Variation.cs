using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace PvLdWebApi.Models.Ld;

public class Variation
{
    [JsonPropertyName("_id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("value")]
    public JsonValue Value { get; set; }
}