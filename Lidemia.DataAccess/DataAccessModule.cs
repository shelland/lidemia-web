// Created on 01/09/2026 16:34 by Laserson

using Lidemia.Core.Extensions;
using Lidemia.DataAccess.Context;
using Lidemia.DataAccess.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lidemia.DataAccess;

public static class DataAccessModule
{
    public static IServiceCollection AddDataAccessModule(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Main").NotNull();
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