// Created on 25/12/2025 18:05 by Laserson

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Lidemia.Core.Extensions;

public static class RequestExtensions
{
    public static Guid? GetCorrelationId(this HttpRequest request)
    {
        var id = request.Headers[Constants.CorrelationIdHeader].NotNull();

        if (id == StringValues.Empty)
        {
            return null;
        }

        return Guid.Parse(id);
    }
}