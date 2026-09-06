// Created on 04/09/2026 20:17 by Laserson

using Microsoft.Extensions.DependencyInjection;

namespace Lidemia.Common;

public static class CommonModule
{
    public static IServiceCollection AddCommonModule(this IServiceCollection service)
    {
        service.Scan(x =>
        {
            x.FromAssemblyOf<ICommonModule>()
                .AddClasses()
                .UsingAttributes();
        });

        return service;
    }
}

public interface ICommonModule;