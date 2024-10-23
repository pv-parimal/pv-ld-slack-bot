using PvLdWebApi.Abstraction.Services;
using PvLdWebApi.Request;

namespace PvLdWebApi.Services;

public class FireAndForgetRunner (IServiceScopeFactory serviceScopeFactory) : IFireAndForgetRunner
{
    public void CreateFlag(FlagCreateRequest request, SlackCommandRequest slackCommand)
    {
        Task.Run(async () =>
        {
            using var scope = serviceScopeFactory.CreateScope();
            var ldClient = scope.ServiceProvider.GetRequiredService<ILdClient>();
            var slackClient = scope.ServiceProvider.GetRequiredService<ISlackClient>();
            try
            {
                
                await ldClient.CreateFlagAsync(request);
                var message = $"<@{slackCommand.UserId}> Flag `{request.Key}` created successfully.";
                await slackClient.ReplyToUrlAsync(slackCommand.ResponseUrl, message);
            }
            catch (Exception e)
            {
                var message = $"""
                               <@{slackCommand.UserId}> An error occured while trying to create flag `{request.Key}`.

                               Error: {e.Message}
                               Stack Trace: {e.StackTrace}
                               """;
                await slackClient.ReplyToUrlAsync(slackCommand.ResponseUrl, message);
            }
        });
    }
}