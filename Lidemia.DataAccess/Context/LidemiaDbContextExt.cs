// Created on 01/09/2026 13:34 by Laserson

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Lidemia.DataAccess.Abstract;

namespace Lidemia.DataAccess.Context;

public partial class LidemiaDbContext
{
    public static ILoggerFactory Logger { get; }

    static LidemiaDbContext()
    {
        Logger = LoggerFactory.Create(x =>
        {
            x.ClearProviders();
            x.AddDebug();
        });
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        UpdateTrackingDates();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        UpdateTrackingDates();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTrackingDates()
    {
        foreach (var entry in ChangeTracker.Entries<IDbEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreateDate = DateTime.UtcNow;
                entry.Entity.IsActive = true;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdateDate = DateTime.UtcNow;
                entry.Entity.RowVersion++;
            }
        }
    }

}