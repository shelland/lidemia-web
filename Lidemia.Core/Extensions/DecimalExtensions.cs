// Created on 18/11/2026 22:51 by Laserson

using Lidemia.Core.Enums;
using Lidemia.Core.Models.Misc;

namespace Lidemia.Core.Extensions;

public static class DecimalExtensions
{
    public static Price ToPrice(this decimal value, Currency currency = Currency.RussianRouble)
    {
        return new Price(Value: value, Currency: currency);
    }
}