// Created on 05/03/2020 18:09 by Andrey Laserson

using System.Globalization;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.WebEncoders;

namespace Lidemia.Logic.Extensions;

public static class ServiceLocalizationExtensions
{
    public static void RegisterLocalization(this IServiceCollection services)
    {
        services.Configure<WebEncoderOptions>(opts =>
        {
            opts.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
        });

        services.AddLocalization();

        services.Configure<RequestLocalizationOptions>(opts =>
        {
            var supportedCultures = new List<CultureInfo>
            {
                new("ru"),
                new("en")
            };

            opts.DefaultRequestCulture = new RequestCulture("en");

            opts.SupportedCultures = supportedCultures;
            opts.SupportedUICultures = supportedCultures;
        });
    }
}