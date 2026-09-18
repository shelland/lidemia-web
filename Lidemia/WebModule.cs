// Created on 15/09/2026 18:50 by Laserson

namespace Lidemia;

public static class WebModule
{
    public static IServiceCollection AddWebModule(this IServiceCollection service)
    {
        service.Scan(x => x.FromAssemblyOf<IWebModule>().AddClasses().UsingAttributes());
        return service;
    }
}

public interface IWebModule;