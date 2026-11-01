// Created on 28/09/2026 21:53 by Laserson

using FluentResults;
using Lidemia.Core.Models.Service;

namespace Lidemia.Core;

public static class Errors
{
    public static ErrorResultInfo InvalidCredentials { get; } = new(["errors.signin.invalidCredentials"]);

    public static IError UnauthorizedError { get; } = new Error("errors.unauthorized");
}