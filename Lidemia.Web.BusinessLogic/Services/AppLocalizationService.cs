// Created on 29/12/2021 22:09 by shell

using System.Collections.Immutable;
using System.Globalization;
using Lidemia.Core.Models.Misc;
using Lidemia.Web.BusinessLogic.Services.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.Web.BusinessLogic.Services;

[ServiceDescriptor<IAppLocalizationService>(ServiceLifetime.Singleton)]
public class AppLocalizationService : IAppLocalizationService
{
    private readonly ImmutableHashSet<string> supportedCultures = new HashSet<string>(["ru", "en"]).ToImmutableHashSet();

    public AppCulture CurrentCulture
    {
        get
        {
            var culture = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;

            if (!supportedCultures.Contains(culture))
            {
                return new AppCulture("en", "MM/dd/yyyy", "m/d/Y", "m/d/Y h:i K");
            }

            return new AppCulture(culture, "dd/MM/yyyy", "d/m/Y", "d/m/Y H:i");
        }
    }

    public CultureInfo CurrentCultureInfo { get; } = Thread.CurrentThread.CurrentCulture;
}