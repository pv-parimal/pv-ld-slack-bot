namespace PvLdWebApi.Extensions;

public static class FormattingExtensions
{
    // write extension method to convert boolean to Yes or No
    public static string ToYesNo(this bool value)
    {
        return value ? "Yes" : "No";
    }
}