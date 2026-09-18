// Created on 17/09/2026 21:52 by Laserson

namespace Lidemia.DataAccess.Entities;

public class SupplierStatsEntity
{
    public long SupplierId { get; set; }

    public int TotalOrders { get; set; }

    public double AverageRating { get; set; }

    public int TotalProducts { get; set; }
}