// Created on 14/09/2026 20:27 by Laserson

using System.Text;
using System.Text.RegularExpressions;

namespace Lidemia.Common.Logic;

public partial class SlugHelper
{
    private static readonly Dictionary<string, string> CyrillicToLatinMap = new()
    {
        {"а", "a"}, {"б", "b"}, {"в", "v"}, {"г", "g"}, {"д", "d"},
        {"е", "e"}, {"ё", "yo"}, {"ж", "zh"}, {"з", "z"}, {"и", "i"},
        {"й", "y"}, {"к", "k"}, {"л", "l"}, {"м", "m"}, {"н", "n"},
        {"о", "o"}, {"п", "p"}, {"р", "r"}, {"с", "s"}, {"т", "t"},
        {"у", "u"}, {"ф", "f"}, {"х", "kh"}, {"ц", "ts"}, {"ч", "ch"},
        {"ш", "sh"}, {"щ", "shch"}, {"ъ", ""}, {"ы", "y"}, {"ь", ""},
        {"э", "e"}, {"ю", "yu"}, {"я", "ya"}
    };


    [GeneratedRegex(@"[^a-z0-9\s\-]")]
    private static partial Regex ReplaceInvalidChars();

    [GeneratedRegex(@"[\s\-]+")]
    private static partial Regex ConvertHyphens();

    public static string CreateTransliteratedSlug(string phrase)
    {
        if (string.IsNullOrWhiteSpace(phrase))
        {
            return string.Empty;
        }

        string str = phrase.ToLowerInvariant();

        StringBuilder sb = new();

        foreach (char c in str)
        {
            string key = c.ToString();

            if (CyrillicToLatinMap.TryGetValue(key, out var latinValue))
            {
                sb.Append(latinValue);
            }
            else
            {
                sb.Append(c);
            }
        }

        str = sb.ToString();

        str = ReplaceInvalidChars().Replace(str, "");
        str = ConvertHyphens().Replace(str, "");

        return str.Trim('-');
    }
}