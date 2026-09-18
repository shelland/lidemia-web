// Created on 14/09/2026 20:22 by Laserson

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lidemia.EmailNotifications;

public static class EmailNotificationsModule
{
    public static IServiceCollection AddEmailNotificationsModule(this IServiceCollection service, IConfiguration configuration)
    {
        service.Scan(x => x.FromAssemblyOf<IEmailNotificationsModule>().AddClasses().UsingAttributes());
        return service;
    }
}

public interface IEmailNotificationsModule;