using System.Text;
using Microsoft.AspNetCore.Mvc;
using PvLdWebApi.Abstraction.Services;
using PvLdWebApi.Extensions;
using PvLdWebApi.Mediator;
using PvLdWebApi.Models.Ld;
using PvLdWebApi.Request;

namespace PvLdWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class FeatureFlagController(ILdClient ldClient, IFireAndForgetRunner backgroundTaskService) : ControllerBase
{
    private const int MaxFlagListLimit = 10;
    private const string HelpText = """
                                    -----------------------------------------
                                    Planview LaunchDarkly Slack Bot
                                    -----------------------------------------

                                    `/pvld list [env={env_key}] [filter={pattern}]` - List all feature flags filtered by globbing pattern.
                                    `/pvld switch [env={flag_key}] [flag={flag_key}] [value={true|false}]` - Switch feature flag on or off.
                                    `/pvld create [key={flag_key}] [name={flag_name}] [description={FLAG_DESCRIPTION}]` - Create a new feature flag.
                                    
                                    *Note:* Values with spaces should be enclosed in double quotes.
                                    """;
    // write method to handle Slack Command
    [HttpPost]
    public async Task<IActionResult> Post([FromForm] SlackCommandRequest request)
    {
        var args = request.Text.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        Command command;
        try
        {
            command = Command.Parse(args);
        }
        catch (Exception e)
        {
            const string errorMessage = "Invalid command. See help text below.";
            var response = GetResponse($"{errorMessage}\r\n\r\n{HelpText}");
            return Ok(response);
        }

        if (args.Length == 0)
        {
            var errorMessage = $"<@{request.UserId}> No command found. See usage help below";
            var response = GetResponse($"{errorMessage}\r\n\r\n{HelpText}");
            return Ok(response);
        }

        switch (command)
        {
            case ListCommand listCommand:
                var flags = new List<FlagInfoBase>();
                if (!string.IsNullOrEmpty(listCommand.Environment))
                {
                    var results = await ldClient.FlagsForEnvAsync(listCommand.Environment, listCommand.Filter);
                    flags.AddRange(results);
                }
                else
                {
                    var results = await ldClient.FlagsAsync(listCommand.Filter);
                    flags.AddRange(results);
                }
                
                var sb = new StringBuilder();
                sb.AppendLine("List of available feature flags:");
                sb.AppendLine();
                foreach (var flag in flags.Take(MaxFlagListLimit))
                {
                    sb.Append($"• `{flag.Key}` [Name: {flag.Name}]");
                    if (flag is FlagInfoDetail detail)
                    {
                        var envInfo = detail.Environments[listCommand.Environment!];
                        sb.Append($" [Flag Enabled: {envInfo.On.ToYesNo()}]");
                    }
                    
                    sb.AppendLine();
                };
                if (flags.Count > MaxFlagListLimit)
                {
                    sb.AppendLine();
                    sb.AppendLine($"Note: Only showing first {MaxFlagListLimit} flags. Use filter to see more flags.");
                }
                return Ok(GetResponse(sb.ToString()));
            case SwitchCommand switchCommand:
                await ldClient.SwitchFlagAsync(switchCommand.Environment, switchCommand.Flag, switchCommand.Value);
                return Ok(GetResponse($"<@{request.UserId}> Your request to switch `{(switchCommand.Value ? "On" : "Off")}` feature flag `{switchCommand.Flag}` is completed."));
            case CreateCommand createCommand:
                backgroundTaskService.CreateFlag(new FlagCreateRequest(createCommand.Key, createCommand.Name, createCommand.Description), request);
                return Ok(GetResponse($"<@{request.UserId}> Your request to create feature flag `{createCommand.Key}` is being processed. You will be notified once it is completed."));
            default:
                var errorMessage = "Invalid command. See help text below.";
                var response = GetResponse($"{errorMessage}\r\n\r\n{HelpText}");
                return Ok(response);
            
        }
    }
    
    private static Dictionary<string, string> GetResponse (string message)
    {
        return new Dictionary<string, string>
        {
            { "response_type", "in_channel" },
            { "text", message }
        };
    }
    
    // private static bool IsMatchingPattern(FlagInfoBase flag, string? pattern)
    // {
    //     if (string.IsNullOrEmpty(pattern))
    //     {
    //         return true;
    //     }
    //     return flag.Key.Like(pattern) || flag.Name.Like(pattern);
    // }
}
