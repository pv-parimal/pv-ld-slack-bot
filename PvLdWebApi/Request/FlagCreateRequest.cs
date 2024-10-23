namespace PvLdWebApi.Request;

public class FlagCreateRequest (string key, string name, string description)
{
    public string Key { get; set; } = key;

    public string Name { get; set; } = name;

    public string Description { get; set; } = description;
}