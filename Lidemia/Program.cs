using Lidemia.Common.Logic.Logging;
using Lidemia.Core;
using Lidemia.Core.Extensions;
using Lidemia.Core.Models.Configuration;
using Lidemia.Logic;
using Lidemia.Logic.Extensions;
using Lidemia.ServiceDefaults;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc.Razor;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Lidemia;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

        builder.Configuration.AddEnvironmentVariables();

        builder.Services.Configure<AppIdSettings>(builder.Configuration.GetSection("AppId"));

        builder.Services
            .AddDataProtection()
            .PersistKeysToFileSystem(new DirectoryInfo(builder.Configuration.GetValue<string>("Application:EncryptionKeysDirectory").NotNull()))
            .SetApplicationName(Constants.AppName);

        // Add services to the container.
        builder.Services.AddControllersWithViews()
            .AddJson()
            .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
            .AddRazorRuntimeCompilation();

        builder.Services.RegisterLogging(builder.Configuration);
        builder.Services.RegisterAuth(builder.Configuration);
        builder.Services.RegisterLocalization();

        builder.Services.AddHealthChecks().AddCheck<DbHealthCheck>("DbHealth");

        builder.Logging.AddOpenTelemetry(options =>
        {
            options
                .SetResourceBuilder(
                    ResourceBuilder.CreateDefault()
                        .AddService("lidemia-web-ui"))
                .AddConsoleExporter();
        });

        builder.Services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService("lidemia-web-ui"))
            .WithTracing(tracing => tracing
                .AddAspNetCoreInstrumentation()
                .AddConsoleExporter())
            .WithMetrics(metrics =>
            {
                metrics
                    .AddAspNetCoreInstrumentation()
                    .AddConsoleExporter().AddPrometheusExporter().AddMeter("Microsoft.AspNetCore.Hosting",
                        "Microsoft.AspNetCore.Server.Kestrel");
            });


        builder.Services.AddProfiler();
        builder.Services.RegisterModules(builder.Configuration);

        var app = builder.Build();

        app.MapDefaultEndpoints();

        app.UseWhen(x => !x.Request.Path.StartsWithSegments("/metrics"), x => x.UseMiddleware<CorrelationIdMiddleware>());

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
        }

        app.UseStaticFiles();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseRequestLocalization();

        app.MapStaticAssets();
        app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
            .WithStaticAssets();

        app.MapPrometheusScrapingEndpoint().DisableHttpMetrics();
        app.MapHealthChecks("/healthz").DisableHttpMetrics();

        await app.RunMigrations();
        await app.RunAsync();
    }
}