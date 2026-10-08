// Created on 01/09/2026 13:34 by Laserson

using Lidemia.DataAccess.Abstract;
using LinqToDB.Data;
using LinqToDB.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Lidemia.DataAccess.Context;

public partial class LidemiaDbContext
{
    public static BulkCopyOptions ProviderBulk { get; } = new()
    {
        BulkCopyType = BulkCopyType.ProviderSpecific
    };

    public static ILoggerFactory Logger { get; }

    static LidemiaDbContext()
    {
        LinqToDBForEFTools.EnableChangeTracker = false;
        LinqToDBForEFTools.Initialize();

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