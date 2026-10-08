// Created on 08/10/2026 18:34 by Laserson

using Lidemia.Core.Enums;

namespace Lidemia.Core.Exceptions;

public class AppFlowException : Exception
{
    public AppFlowException(AppFlowExceptionType type, string? details = null)
    {
        Type = type;
        Details = details;
    }

    public AppFlowExceptionType Type { get; }

    public string? Details { get; }

    public static AppFlowException Unauthorized { get; } = new AppFlowException(AppFlowExceptionType.Unauthorized);
}