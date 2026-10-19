// Created on 09/10/2026 18:46 by Laserson

using Ganss.Xss;
using Lidemia.Common.BusinessLogic.Services.App.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.Common.BusinessLogic.Services.App;

[ServiceDescriptor<ISanitizerService>(ServiceLifetime.Singleton)]
public class SanitizerService : ISanitizerService
{
    public string? Sanitize(string? input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return null;
        }

        var sanitizer = new HtmlSanitizer();
        var result = sanitizer.Sanitize(input);

        return result;
    }
}