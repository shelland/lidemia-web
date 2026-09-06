// Created on 03/09/2026 19:29 by Laserson

using Microsoft.Extensions.DependencyInjection;

namespace Lidemia.Web.BusinessLogic;

public static class WebBusinessLogicModule
{
    public static IServiceCollection AddWebBusinessLogicModule(this IServiceCollection services)
    {
        services.Scan(x =>
        {
            x.FromAssemblyOf<IWebBusinessLogicModule>()
                .AddClasses().UsingAttributes();
        });

        return services;
    }
}

public interface IWebBusinessLogicModule;