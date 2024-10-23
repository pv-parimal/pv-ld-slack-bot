namespace PvLdWebApi.Abstraction.Services;

public interface ISlackClient
{
    Task ReplyToUrlAsync(string responseUrl, string message);
}