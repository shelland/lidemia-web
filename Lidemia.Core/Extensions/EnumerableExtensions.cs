// Created on 27/11/2026 18:40 by Laserson

namespace Lidemia.Core.Extensions;

public static class EnumerableExtensions
{
    public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T>? source)
        => source ?? [];
}