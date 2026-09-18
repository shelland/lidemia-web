// Created on 02/09/2026 21:20 by Laserson

using Lidemia.DataAccess.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Lidemia.DataAccess.Context;

public class LidemiaDbContextFactory : IDesignTimeDbContextFactory<LidemiaDbContext>
{
    public LidemiaDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<LidemiaDbContext>();
        const string connectionString = "Host=127.0.0.1;Port=5432;Database=lidemia-dev;Username=postgres;Password=postgres";

        optionsBuilder.ConfigureDbContext(connectionString);
        return new LidemiaDbContext(optionsBuilder.Options);
    }
}