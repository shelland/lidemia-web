// Created on 20/02/2021 19:29 by Andrey Laserson

using System.Runtime.CompilerServices;

namespace Lidemia.Core.Extensions;

public static class ObjectExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: System.Diagnostics.CodeAnalysis.NotNull]
    public static T NotNull<T>(this T? obj, [CallerArgumentExpression(nameof(obj))] string msg = "") => obj != null ? obj : throw new NullReferenceException(msg);
}