using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using PvLdWebApi.Abstraction.Services;
using PvLdWebApi.Models.Ld;
using PvLdWebApi.Request;

namespace PvLdWebApi.Services;

public class LdClient(HttpClient client) : ILdClient
{
    // private const string Project = "clarizen";
    private const string Project = "default";

    public async Task<List<FlagInfoBase>> FlagsAsync(string? filter= null)
    {
        var url = $"https://app.launchdarkly.com/api/v2/flags/{Project}";
        if (!string.IsNullOrEmpty(filter))
        {
            url += $"?filter=query:{filter}";
        }
        var httpResponse = await client.GetAsync(url);
        var result = await httpResponse.Content.ReadFromJsonAsync<LdListResult<FlagInfoBase>>();
        return result!.Items;
    }

    public async Task<List<FlagInfoDetail>> FlagsForEnvAsync(string? env = null, string? filter= null)
    {
        var url = $"https://app.launchdarkly.com/api/v2/flags/{Project}?env={env}";
        if (!string.IsNullOrEmpty(filter))
        {
            url += $"&filter=query:{filter}";
        }
        var httpResponse = await client.GetAsync(url);
        var result = await httpResponse.Content.ReadFromJsonAsync<LdListResult<FlagInfoDetail>>();
        return result!.Items;
    }

    public async Task<FlagInfoBase> FlagInfoAsync(string flagKey)
    {
        var url = $"https://app.launchdarkly.com/api/v2/flags/{Project}/{flagKey}";
        var httpResponse = await client.GetAsync(url);
        var result = await httpResponse.Content.ReadFromJsonAsync<FlagInfoBase>();
        return result!;
    }

    public async Task SwitchFlagAsync(string env, string flag, bool value)
    {
        var url = $"https://app.launchdarkly.com/api/v2/flags/{Project}/{flag}?ignoreConflicts=true";
        var request = new LdPatchRequest("replace", $"/environments/{env}/on", value);
        var json = JsonSerializer.Serialize(new List<LdPatchRequest> { request });
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var httpResponse = await client.PatchAsync(url, content);
        httpResponse.EnsureSuccessStatusCode();
    }

    public async Task CreateFlagAsync(FlagCreateRequest request)
    {
        var payload = LdFlagCreate<bool>.CreateBoolFlag(request.Key, request.Name, request.Description);
        var json = JsonSerializer.Serialize(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var httpResponse = await client.PostAsync($"https://app.launchdarkly.com/api/v2/flags/{Project}", content);
        httpResponse.EnsureSuccessStatusCode();
    }

    private class LdPatchRequest(string operation, string path, object value)
    {
        [JsonPropertyName("op")]
        public string Operation { get; set; } = operation;

        [JsonPropertyName("path")]
        public string Path { get; set; } = path;

        [JsonPropertyName("value")]
        public object Value { get; set; } = value;
    }
}