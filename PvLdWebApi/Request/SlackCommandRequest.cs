using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace PvLdWebApi.Request;

public class SlackCommandRequest
{
    [Required]
    [ModelBinder(Name = "command")]
    public string Command { get; set; } = string.Empty;

    [Required]
    [ModelBinder(Name = "text")]
    public string Text { get; set; } = string.Empty;

    [Required]
    [ModelBinder(Name = "user_id")]
    public string UserId { get; set; } = string.Empty;

    [Required]
    [ModelBinder(Name = "user_name")]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    [ModelBinder(Name = "channel_id")]
    public string ChannelId { get; set; } = string.Empty;
    
    [Required]
    [ModelBinder(Name = "channel_name")]
    public string ChannelName { get; set; } = string.Empty;
    
    [Required]
    [ModelBinder(Name = "response_url")]
    public string ResponseUrl { get; set; } = string.Empty;
}
