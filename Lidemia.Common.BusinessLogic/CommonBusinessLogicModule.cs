// Created on 02/09/2026 20:11 by Laserson

using Microsoft.Extensions.DependencyInjection;

namespace Lidemia.Common.BusinessLogic;

public static class CommonBusinessLogicModule
{
    public static IServiceCollection AddCommonBusinessLogicModule(this IServiceCollection service)
    {
        service.Scan(x => x.FromAssemblyOf<ICommonBusinessLogicModule>().AddClasses().UsingAttributes());
        return service;
    }
}

public interface ICommonBusinessLogicModule;