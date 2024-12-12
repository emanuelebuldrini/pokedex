
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
        return Regex.Replace(translation, @"\s{2,}", " ");
    }
}