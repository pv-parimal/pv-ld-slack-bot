using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using PvLdWebApi.Helpers;

namespace PvLdWebApi.Mediator;


[JsonPolymorphic(TypeDiscriminatorPropertyName = "command")]
[JsonDerivedType(typeof(ListCommand), typeDiscriminator: "list")]
[JsonDerivedType(typeof(SwitchCommand), typeDiscriminator: "switch")]
[JsonDerivedType(typeof(CreateCommand), typeDiscriminator: "create")]
public class Command
{
    private static readonly Regex RegexArgs = new(@"(\w+)=(""[^""]*""|\S+)", RegexOptions.Compiled);
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        Converters = { new JsonBooleanConverter() }
    };
    public static Command Parse(string[] args)
    {
        var dictionary = new Dictionary<string, object>
        {
            { "command", args[0] }
        };

        if (args.Length == 2)
        {
            var matches = RegexArgs.Matches(args[1]);
            foreach (Match match in matches)
            {
                var key = match.Groups[1].Value;
                var value = match.Groups[2].Value;
                if (value.StartsWith("\"") && value.EndsWith("\""))
                {
                    value = value[1..^1];
                }
                dictionary[key] = value;
            }
        }

        var jsonString = JsonSerializer.Serialize(dictionary);
        return JsonSerializer.Deserialize<Command>(jsonString, SerializerOptions)!;
    }
}