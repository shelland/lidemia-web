// Created on 26/09/2026 19:04 by Laserson

using Lidemia.ExternalServices.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Lidemia.ExternalServices;

public static class ExternalServicesModule
{
    public static IServiceCollection AddExternalServicesModule(this IServiceCollection services)
    {
        services.AddHttpClient(nameof(HttpEmailService));
        services.AddHttpClient(nameof(RecaptchaService));

        return services;
    }
}