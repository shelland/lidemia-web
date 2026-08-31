// Created on 20/11/2025 20:47 by Laserson

using Lidemia.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.Common.Logic.Logging;

[ServiceDescriptor<CorrelationIdMiddleware>(ServiceLifetime.Singleton)]
public class CorrelationIdMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var correlationId = Guid.NewGuid().ToString();

        if (!context.Request.Headers.ContainsKey(Constants.CorrelationIdHeader))
        {
            context.Request.Headers[Constants.CorrelationIdHeader] = correlationId;
        }

        context.Items[Constants.CorrelationIdHeader] = correlationId;
        context.Response.OnStarting(() =>
        {
            context.Response.Headers[Constants.CorrelationIdHeader] = correlationId;
            return Task.CompletedTask;
        });

        await next(context);
    }
}