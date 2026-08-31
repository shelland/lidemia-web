using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Lidemia.Core.Extensions;

public static class StringExtensions
{
    public static string GetSecureHash(this string input)
    {
        var encoding = new UnicodeEncoding();

        var bytes = encoding.GetBytes(input);
        var hash = SHA512.HashData(bytes);

        return Convert.ToBase64String(hash);
    }

    public static decimal ToDecimalInvariant(this string str)
    {
        return decimal.Parse(str, CultureInfo.InvariantCulture);
    }

    public static double ToDoubleInvariant(this string str)
    {
        return double.Parse(str, CultureInfo.InvariantCulture);
    }

    public static string JoinForLog(this IEnumerable<string> src, string glue = ";\r\n")
    {
        return string.Join(glue, src);
    } 
}