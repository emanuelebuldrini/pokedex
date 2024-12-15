
using System.Text.RegularExpressions;

namespace Pokedex.Application.Shared;

public class Utils
{
    public static string SanitizeFlavorText(string? flavorText)
    {
        if (flavorText == null)
        {
            return string.Empty;
        }

        // Replace line endings like \n, \f with a space.
        return flavorText.ReplaceLineEndings(" ");
    }

    public static string SanitizeTranslation(string translation)
    {
        // Replace multiple spaces with a single space
        var sanitizedSpaces = Regex.Replace(translation, @"\s{2,}", " ");

        // Add a space after a full stop that separates two sentences.
        var sanitizedFullStops = Regex.Replace(sanitizedSpaces, @"(?<=\w)\.(?=\w)", ". ");

        // Add a space after a comma that separates two sentences.
        var sanitizedCommas = Regex.Replace(sanitizedFullStops, @"(?<=\w)\,(?=\w)", ", ");

        // Make sure that the sentence after a comma is not capitalized.
        return Regex.Replace(sanitizedCommas, @"(?<=,\s)([A-Z])", m => m.Value.ToLower());
    }
}