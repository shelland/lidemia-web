// Created on 20/11/2021 17:02 by Andrey Laserson

using System.Reflection;
using Lidemia.DataAccess.Context;
using Lidemia.DataAccess.Logic.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Lidemia.DataAccess.Extensions;

public static class DbContextOptionsBuilderExtensions
{
    public static void ConfigureDbContext(this DbContextOptionsBuilder builder, string connectionString, IServiceProvider? sp)
    {
        builder.EnableSensitiveDataLogging();
        builder.EnableDetailedErrors();

        var dataBuilder = new NpgsqlDataSourceBuilder(connectionString);
        dataBuilder.EnableDynamicJson();
        dataBuilder.EnableParameterLogging();
        var dataSource = dataBuilder.Build();

        var optionsBuilder = builder.UseNpgsql(dataSource, db =>
            {
                db.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
                db.UseNetTopologySuite(geographyAsDefault: true);
                db.EnableRetryOnFailure(3);
            })
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            .UseSnakeCaseNamingConvention()
            .UseLoggerFactory(LidemiaDbContext.Logger);

        if (sp == null)
        {
            return;
        }

        var slowQueryInterceptor = sp.GetRequiredService<SlowQueriesInterceptor>();
        optionsBuilder.AddInterceptors(slowQueryInterceptor);
    }
}