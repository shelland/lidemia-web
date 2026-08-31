// Created on 29/11/2021 22:12 by Andrey Laserson

using Lidemia.Core.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Filters;
using Serilog.Sinks.SystemConsole.Themes;

namespace Lidemia.Common.Logic.Logging;

public static class LoggingServiceCollectionExtensions
{
    public static void RegisterLogging(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var seqUrl = configuration.GetValue<string>("LocalServices:Seq:Url");
        var seqApiKey = configuration.GetValue<string>("LocalServices:Seq:ApiKey");

        const string outputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}][{SourceContext}] {Message:lj}{NewLine}{Exception}";

        var logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .Enrich.FromLogContext()
            .Enrich.With<CorrelationIdEnricher>()
            .WriteTo.Console(outputTemplate: outputTemplate, theme: AnsiConsoleTheme.Code)
            .WriteTo.Seq(seqUrl.NotNull(), apiKey: seqApiKey)
            .Filter.ByExcluding(Matching.FromSource("Microsoft"))
            .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore"))
            .Filter.ByExcluding(Matching.FromSource("System"))
            .CreateLogger();

        serviceCollection.AddLogging(cfg =>
        {
            cfg.ClearProviders();
            cfg.AddSerilog(logger);
        });
    }
}