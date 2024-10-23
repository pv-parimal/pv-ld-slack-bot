using System.Text.Json.Serialization;

namespace PvLdWebApi.Mediator;

public class ListCommand : Command
{
    [JsonPropertyName("env")]
    public string? Environment { get; set; }

    [JsonPropertyName("filter")]
    public string? Filter { get; set; }
}