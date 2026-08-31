// Created on 20/11/2025 20:44 by Laserson

using Lidemia.Core.Extensions;
using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;

namespace Lidemia.Common.Logic.Logging;

public class CorrelationIdEnricher : ILogEventEnricher
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public CorrelationIdEnricher() : this(new HttpContextAccessor())
    {
    }

    public CorrelationIdEnricher(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        logEvent.AddOrUpdateProperty(new LogEventProperty("App", new ScalarValue(Core.Constants.AppName)));

        if (this.httpContextAccessor.HttpContext == null)
        {
            return;
        }

        if (!this.httpContextAccessor.HttpContext.NotNull().Request.Headers.TryGetValue(Core.Constants.CorrelationIdHeader, out var correlationId))
        {
            return;
        }

        logEvent.AddOrUpdateProperty(new LogEventProperty("CorrelationId", new ScalarValue(correlationId)));
    }
}