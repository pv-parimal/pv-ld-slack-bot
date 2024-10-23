using System.Text.Json.Serialization;

namespace PvLdWebApi.Models.Ld;

public class LdListResult<T>
{
    [JsonPropertyName("items")]
    public List<T> Items { get; set; } = [];
}