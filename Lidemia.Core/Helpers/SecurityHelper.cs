// Created on 13/05/2020 23:22 by Andrey Laserson

namespace Lidemia.Core.Helpers;

public static class SecurityHelper
{
    static SecurityHelper()
    {
        CapitalChars = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'X', 'Y', 'Z'];
    }

    //public static int GenerateOrderNumber()
    //{


    //    var ms = (int)DateTimeOffset.Now.ToUnixTimeSeconds() << Guid.NewGuid().GetHashCode();
    //    var rnd = new Random(ms);

    //    return rnd.Next(100_000_000, 999_999_999);
    //}

    public static string GenerateRandomCode(int length)
    {
        var result = string.Empty;
        var rnd = new Random(Guid.NewGuid().GetHashCode());

        string[] chars =
        [
            "A", "a", "B", "b", "C", "c", "D", "d", "E", "e", "F", "f", "G", "g", "H", "h", "I", "i", "J", "j", "K", "k", "L", "l", "M", "m", "N", "n", "O",
            "o", "P", "p", "Q", "q", "R", "r", "S", "s", "T", "t", "U", "u", "V", "v", "W", "w", "X", "x", "Y", "y", "Z", "z", "1", "2", "3", "4", "5", "6",
            "7", "8", "9", "0"
        ];


        for (var i = 0; i < length; i++)
        {
            result += chars[rnd.Next(0, chars.Length)];
        }

        return result;
    }

    public static string GeneratePin()
    {
        var seed = Guid.NewGuid().GetHashCode();
        var random = new Random(seed).Next(1, 9999);

        return $"{random:0000}";
    }

    public static char[] CapitalChars { get; }
}