// Created on 01/04/2020 0:30 by Andrey Laserson

using FluentResults;
using FluentValidation.Results;

namespace Lidemia.Core.Extensions;

public static class ValidationExtensions
{
    public static List<string> GetErrors(this ValidationResult result)
    {
        return result.Errors.Select(x => x.ErrorCode).ToList();
    }

    public static IReadOnlyList<Error> GetResultErrors(this ValidationResult result)
    {
        return result.Errors.Select(x => new Error(x.ErrorCode)).ToList();
    }
}