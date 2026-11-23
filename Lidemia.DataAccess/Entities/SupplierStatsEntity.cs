// Created on 17/09/2026 21:52 by Laserson

using Lidemia.DataAccess.Abstract;

namespace Lidemia.DataAccess.Entities;

public class SupplierStatsEntity : IDbEntity
{
    public long SupplierId { get; set; }

    public SupplierEntity Supplier { get; set; } = null!;

    public int TotalOrders { get; set; }

    public double AverageRating { get; set; }

    public int TotalProducts { get; set; }

    public int RowVersion { get; set; }

    public bool IsActive { get; set; }

    public DateTimeOffset CreateDate { get; set; }

    public DateTimeOffset? UpdateDate { get; set; }
}