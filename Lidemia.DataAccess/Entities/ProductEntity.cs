// Created on 01/09/2026 13:29 by Laserson

using Lidemia.Core.Enums;
using Lidemia.Core.Models.Base;
using Lidemia.DataAccess.Abstract;

namespace Lidemia.DataAccess.Entities;

public class ProductEntity : IHasId<long>, IDbEntity
{
    public long Id { get; set; }

    public long? CategoryId { get; set; }

    public ProductCategoryEntity? Category { get; set; }

    public long SupplierId { get; set; }

    public SupplierEntity Supplier { get; set; } = null!;

    public long? ParentId { get; set; }

    public ProductEntity? Parent { get; set; }

    public string? Title { get; set; }

    public string? ShortDescription { get; set; }

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public long? PrimaryPhotoId { get; set; }

    public PhotoEntity? PrimaryPhoto { get; set; }

    public bool IsDiscount { get; set; }

    public bool HasCurrentDiscount { get; set; }

    public DateTime? DiscountEndDate { get; set; }

    public DateTime? DiscountStartDate { get; set; }

    public decimal? DiscountPrice { get; set; }

    public double? MinQuantity { get; set; }

    public double? MaxQuantity { get; set; }

    public bool IsVisible { get; set; }

    public string? Slug { get; set; }

    public double? AverageRatingRecent { get; set; }

    public double? AverageRatingOverall { get; set; }

    public int TotalRates { get; set; }

    public DateTime? FirstSeenDate { get; set; }

    public ProductPriceUnit? PriceUnit { get; set; }

    public ProductAvailabilityType? AvailabilityType { get; set; }

    public int? OrderRank { get; set; }

    public string? Sku { get; set; }

    public int RowVersion { get; set; }

    public bool IsActive { get; set; }

    public DateTimeOffset CreateDate { get; set; }

    public DateTimeOffset? UpdateDate { get; set; }
}