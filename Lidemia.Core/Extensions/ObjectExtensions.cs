// Created on 20/02/2021 19:29 by Andrey Laserson

using Lidemia.Core.Helpers;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace Lidemia.Core.Extensions;

public static class ObjectExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: System.Diagnostics.CodeAnalysis.NotNull]
    public static T NotNull<T>(this T? obj, [CallerArgumentExpression(nameof(obj))] string msg = "") => obj != null ? obj : throw new NullReferenceException(msg);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Serialize<T>(this T? obj) => JsonSerializer.Serialize(obj, CommonJsonOptions.Options);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T? Deserialize<T>(string src) => JsonSerializer.Deserialize<T>(src, CommonJsonOptions.Options);
}