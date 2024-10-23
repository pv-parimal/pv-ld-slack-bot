using System.Text.Json;
using System.Text.Json.Serialization;

namespace PvLdWebApi.Mediator;

public class SwitchCommand : Command
{
    [JsonPropertyName("env")]
    public string Environment { get; set; } = string.Empty;

    [JsonPropertyName("flag")]
    public string Flag { get; set; } = string.Empty;

    [JsonPropertyName("value")]
    public bool Value { get; set; }
}
