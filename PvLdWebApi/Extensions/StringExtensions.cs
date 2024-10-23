using System.Text.RegularExpressions;

namespace PvLdWebApi.Extensions;

public static class StringExtensions
{
    public static bool Like(this string input, string pattern)
    {
        var escapedPattern = Regex.Escape(pattern);
        escapedPattern = escapedPattern.Replace("\\*", ".*");
        escapedPattern = escapedPattern.Replace("\\?", ".");
        escapedPattern = "^" + escapedPattern + "$";
        
        return Regex.IsMatch(input, escapedPattern, RegexOptions.IgnoreCase);
    }
}