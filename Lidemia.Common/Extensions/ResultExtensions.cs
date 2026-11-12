// Created on 09/11/2026 15:49 by Laserson

using FluentResults;

namespace Lidemia.Common.Extensions;

public static class ResultExtensions
{
    public static T GetDefaultError<T>(this IResultBase result) where T : class
    {
        if (!result.IsFailed)
        {
            throw new InvalidOperationException("Result is success");
        }

        return (T)result.Errors[0];
    }
}