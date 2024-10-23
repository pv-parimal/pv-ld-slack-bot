using PvLdWebApi.Models.Ld;
using PvLdWebApi.Request;

namespace PvLdWebApi.Abstraction.Services;

public interface ILdClient
{
    Task<List<FlagInfoBase>> FlagsAsync(string? filter= null);
    Task<List<FlagInfoDetail>> FlagsForEnvAsync(string env, string? filter= null);
    Task<FlagInfoBase> FlagInfoAsync(string flagKey);
    Task SwitchFlagAsync(string env, string flag, bool value);
    
    Task CreateFlagAsync(FlagCreateRequest request);
}
