using System.Text.Json.Serialization;

namespace PvLdWebApi.Models.Ld;

public class LdFlagCreate<T> where T : struct
{
    private LdFlagCreate()
    {
    }

    [JsonPropertyName("key")]
    public string Key { get; set; } = string.Empty;
    
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("variations")]
    public VariationValue<T>[] Variations { get; set; } = [];

    public Dictionary<string, int> Defaults { get; private set; } = new();

    [JsonPropertyName("temporary")]
    public bool Temporary { get; set; } = false;
    
    [JsonPropertyName("environments")]
    public Dictionary<string, FlagValue> Environments { get; set; } = new();
    
    private static Dictionary<string, int> BoolDefaults()
    {
        return new() { { "onVariation", 1 }, { "offVariation", 0 } };
    }
    
    public static LdFlagCreate<bool> CreateBoolFlag(string key, string name, string description)
    {
        return new LdFlagCreate<bool>
        {
            Key = key,
            Name = name,
            Description = description,
            Variations = VariationValue<bool>.BoolVariations(), // TODO: Convert to factory
            Defaults = BoolDefaults()
        };
    }
}
