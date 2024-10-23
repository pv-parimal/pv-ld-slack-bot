using System.Text.Json.Serialization;

namespace PvLdWebApi.Models.Ld;

public class FlagInfoBase
{
    [JsonPropertyName("key")]
    public string Key { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("kind")]
    public string Kind { get; set; }
    
    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("variations")]
    public Variation[] Variations { get; set; } = [];
}
