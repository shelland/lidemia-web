// Created on 18/11/2023 14:12 by shell

using Lidemia.DataAccess.Context;
using Lidemia.DataAccess.Logic.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Scrutor;

namespace Lidemia.DataAccess.Logic;

[ServiceDescriptor<IAppDbMigrator>(ServiceLifetime.Scoped)]
public class AppDbMigrator : IAppDbMigrator
{
    private readonly ILogger<AppDbMigrator> logger;

    public AppDbMigrator(ILogger<AppDbMigrator> logger)
    {
        this.logger = logger;
    }

    public async Task MigrateDatabase(AsyncServiceScope scope)
    {
        this.logger.LogInformation("Running a db migrator...");

        await using var dbContext = scope.ServiceProvider.GetRequiredService<LidemiaDbContext>();
        await dbContext.Database.MigrateAsync();
    }
}