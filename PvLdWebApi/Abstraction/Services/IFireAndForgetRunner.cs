using PvLdWebApi.Request;

namespace PvLdWebApi.Abstraction.Services;

public interface IFireAndForgetRunner
{
    void CreateFlag(FlagCreateRequest request, SlackCommandRequest slackCommand);
}