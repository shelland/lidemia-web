// Created on 02/11/2026 17:35 by Laserson

namespace Lidemia.Admin;

public static class AdminModule
{
    public static IServiceCollection AddAdminModule(this IServiceCollection services)
    {
        services.Scan(x => x.FromAssemblyOf<IAdminModule>().AddClasses().UsingAttributes());
        return services;
    }
}

public interface IAdminModule;