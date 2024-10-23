using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using PvLdWebApi.Abstraction.Services;

namespace PvLdWebApi.Services;

public class SlackClient(HttpClient client) : ISlackClient
{
    public Task ReplyToUrlAsync(string responseUrl, string message)
    {
        var responseMessage = new ResponseMessage
        {
            Text = message
        };
        var json = JsonSerializer.Serialize(responseMessage);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        return client.PostAsync(responseUrl, content);
    }
    
    private class ResponseMessage
    {
        [JsonPropertyName("response_type")]
        public string ResponseType => "in_channel";

        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;
    }
}