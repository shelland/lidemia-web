// Created on 01/09/2026 16:34 by Laserson

using Lidemia.DataAccess.Context;
using Lidemia.DataAccess.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Lidemia.DataAccess;

public static class DataAccessModule
{
    public static IServiceCollection AddDataAccessModule(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<LidemiaDbContext>(x => x.ConfigureDbContext(connectionString));

        services.Scan(x =>
        {
            x.FromAssemblyOf<IDataAccessModule>()
                .AddClasses().UsingAttributes();
        });

        return services;
    }
}

public interface IDataAccessModule;