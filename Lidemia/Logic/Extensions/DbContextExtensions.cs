// Created on 28/02/2023 19:15 by shell

using Lidemia.DataAccess.Logic.Abstract;

namespace Lidemia.Logic.Extensions;

public static class DbContextExtensions
{
    public static async Task RunMigrations(this WebApplication app)
    {
        await using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateAsyncScope();
        var migrators = scope.ServiceProvider.GetRequiredService<IEnumerable<IAppDbMigrator>>();

        foreach (var migrator in migrators)
        {
            await migrator.MigrateDatabase(scope);
        }
    }
}