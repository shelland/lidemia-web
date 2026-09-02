// Created on 02/09/2026 21:20 by Laserson

using Lidemia.DataAccess.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Lidemia.DataAccess.Context;

public class LidemiaDbContextFactory : IDbContextFactory<LidemiaDbContext>
{
    public LidemiaDbContext CreateDbContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<LidemiaDbContext>();
        var connectionString = "Host=127.0.0.1;Port=5432;Database=lidemia-dev;Username=postgres;Password=postgres";

        optionsBuilder.ConfigureDbContext(connectionString);
        return new LidemiaDbContext(optionsBuilder.Options);
    }
}