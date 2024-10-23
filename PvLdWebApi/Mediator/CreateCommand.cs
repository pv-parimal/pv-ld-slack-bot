using System.Text.Json.Serialization;

namespace PvLdWebApi.Mediator;

public class CreateCommand : Command
{
    [JsonPropertyName("key")]
    public string Key { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
}