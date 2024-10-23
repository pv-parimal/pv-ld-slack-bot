using System.Text.Json.Serialization;

namespace PvLdWebApi.Models.Ld;

public class VariationValue<T> where T : struct
{
    [JsonPropertyName("value")]
    public T Value { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    public static VariationValue<bool>[] BoolVariations()
    {
        return
        [
            new VariationValue<bool>
            {
                Value = false,
                Name = "false",
                Description = "Flag is off"
            },
            new VariationValue<bool>
            {
                Value = true,
                Name = "true",
                Description = "Flag is on"
            }
        ];
    }
}