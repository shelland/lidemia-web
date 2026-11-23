// Created on 01/09/2026 16:34 by Laserson

using Lidemia.Core.Extensions;
using Lidemia.DataAccess.Context;
using Lidemia.DataAccess.Extensions;
using Lidemia.DataAccess.Logic.Interceptors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Lidemia.DataAccess;

public static class DataAccessModule
{
    public static IServiceCollection AddDataAccessModule(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Main").NotNull();
        services.AddDbContext<LidemiaDbContext>((sp, x) => x.ConfigureDbContext(connectionString, sp));

        services.Scan(x =>
        {
            x.FromAssemblyOf<IDataAccessModule>()
                .AddClasses().UsingAttributes();
        });

        var slowQueryTimeout = TimeSpan.FromMilliseconds(1000);

        services.AddSingleton<SlowQueriesInterceptor>((sp) =>
        {
            var logger = sp.GetRequiredService<ILogger<SlowQueriesInterceptor>>();
            return new SlowQueriesInterceptor(logger, slowQueryTimeout);
        });

        return services;
    }
}

public interface IDataAccessModule;