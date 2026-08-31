// Created on 04/03/2026 23:12 by Laserson

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Lidemia.Common.Logic;

public static class TelemetryExtensions
{
    public static void AddAppTelemetry(this WebApplicationBuilder builder, string serviceName)
    {
        builder.Services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(serviceName).AddAttributes(new Dictionary<string, object>
            {
                ["deployment.environment"] = builder.Environment.EnvironmentName.ToLower()
            }))
            .WithTracing(tracing => tracing
                .AddAspNetCoreInstrumentation()
                /* .AddConsoleExporter() */)
            .WithMetrics(metrics =>
            {
                metrics
                    .AddAspNetCoreInstrumentation()
                    // .AddConsoleExporter()
                    .AddSqlClientInstrumentation()
                    .AddPrometheusExporter()
                    .AddNpgsqlInstrumentation()
                    .AddMeter("Microsoft.AspNetCore.Hosting", "Microsoft.AspNetCore.Server.Kestrel")
                    .AddMeter("Ubike");
            });

        //builder.Logging.AddOpenTelemetry(logging =>
        //{
        //    logging.IncludeFormattedMessage = true;
        //    logging.IncludeScopes = true;
        //});
    }
}