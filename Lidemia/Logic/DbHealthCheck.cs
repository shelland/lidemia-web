// Created on 18/11/2025 21:47 by Laserson

using Lidemia.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Lidemia.Logic;

public class DbHealthCheck : IHealthCheck
{
    private readonly LidemiaDbContext dbContext;

    public DbHealthCheck(LidemiaDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken)
    {
        try
        {
            await this.dbContext.Database.ExecuteSqlRawAsync("SELECT 1", cancellationToken: cancellationToken);
            return HealthCheckResult.Healthy();
        }
        catch (Exception)
        {
            return HealthCheckResult.Unhealthy();
        }
    }
}