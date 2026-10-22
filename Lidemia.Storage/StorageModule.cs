// Created on 21/10/2026 22:28 by Laserson

namespace Lidemia.Storage;

public static class StorageModule
{
    public static IServiceCollection RegisterStorageModule(this IServiceCollection service)
    {
        service.Scan(x => x.FromAssemblyOf<IStorageModule>().AddClasses().UsingAttributes());
        return service;
    }
}

public interface IStorageModule;