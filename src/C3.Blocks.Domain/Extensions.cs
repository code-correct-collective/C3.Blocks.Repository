using System.Text.RegularExpressions;

namespace C3.Blocks.Domain;

/// <summary>
/// Provides extension methods for string manipulation.
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Converts the specified string to a slug by replacing non-word characters with hyphens and converting to lowercase.
    /// </summary>
    /// <param name="s">The string to convert to a slug.</param>
    /// <returns>A version of the input string with all non-word characters replaced with a dash.</returns>
    public static string ToSlug(this string s)
    {
#pragma warning disable CA1308 // Normalize strings to uppercase
        return Regex.Replace(s, @"\W", "-").ToLowerInvariant();
#pragma warning restore CA1308 // Normalize strings to uppercase
    }
}
