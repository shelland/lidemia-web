// Created on 18/11/2026 22:47 by Laserson

using Lidemia.Core.Models.Misc;

namespace Lidemia.Core.Extensions;

public static class PriceExtensions
{
    public static string GetDisplayValue(this Price? price)
    {
        if (price == null)
        {
            return "[NoPrice]";
        }

        return $"{price.Value} {price.Currency.GetLocalizedDisplayName()}";
    }
}