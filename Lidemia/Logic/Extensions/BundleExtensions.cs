// Created on 28/09/2026 22:02 by Laserson

using Lidemia.Web.BusinessLogic.Services.Abstract;
using Smidge;

namespace Lidemia.Logic.Extensions;

public static class BundleExtensions
{
    public static IServiceCollection AddBundles(this IServiceCollection service)
    {
        service.AddSmidge();
        return service;
    }

    public static WebApplication AddBundles(this WebApplication application)
    {
        var locale = application.Services.GetRequiredService<IAppLocalizationService>().CurrentCulture.Culture;

        application.UseSmidge(x =>
        {
            x.CreateJs("main-js-bundle",
                $"~/js/ClientResources.{locale}.js",
                "~/lib/bootstrap/js/bootstrap.bundle.js",
                "~/lib/microsoft-signalr/signalr.js",
                "~/lib/Trumbowyg/trumbowyg.js"
            );
        });

        return application;
    }
}