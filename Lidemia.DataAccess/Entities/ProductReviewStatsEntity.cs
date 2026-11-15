// Created on 14/11/2026 15:47 by Laserson

using Lidemia.DataAccess.Abstract;
using Lidemia.DataAccess.Entities.Misc;

namespace Lidemia.DataAccess.Entities;

public class ProductReviewStatsEntity : IDbEntity
{
    public long ProductId { get; set; }

    public ProductEntity Product { get; set; } = null!;

    public double? AverageRatingRecent { get; set; }

    public double? AverageRatingOverall { get; set; }

    public int TotalRates { get; set; }

    public ProductReviewRateDistributionDbModel Distribution { get; set; } = null!;

    public int RowVersion { get; set; }

    public bool IsActive { get; set; }

    public DateTimeOffset CreateDate { get; set; }

    public DateTimeOffset? UpdateDate { get; set; }
}