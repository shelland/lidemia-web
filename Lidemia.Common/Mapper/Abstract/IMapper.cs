// Created on 26/10/2026 19:28 by Laserson

namespace Lidemia.Common.Mapper.Abstract;

public interface IMapper<in TFrom, out TTo>
{
    TTo Map(TFrom source);
}

public static class MapperExtensions
{
    public static List<TTo> MapList<TFrom, TTo>(
        this IMapper<TFrom, TTo> mapper,
        IEnumerable<TFrom> source)
        => source.Select(mapper.Map).ToList();

    public static TTo[] MapArray<TFrom, TTo>(
        this IMapper<TFrom, TTo> mapper,
        IEnumerable<TFrom> source)
        => source.Select(mapper.Map).ToArray();

    public static IReadOnlyList<TTo> MapReadOnly<TFrom, TTo>(
        this IMapper<TFrom, TTo> mapper,
        IReadOnlyList<TFrom> source)
        => source.Select(mapper.Map).ToArray();
}