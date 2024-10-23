using System.Text.Json.Serialization;

namespace PvLdWebApi.Models.Ld;

public class FlagInfoDetail : FlagInfoBase
{
    [JsonPropertyName("environments")]
    public Dictionary<string, FlagValue> Environments { get; set; }
}