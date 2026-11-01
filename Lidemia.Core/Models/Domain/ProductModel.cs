// Created on 02/09/2026 19:59 by Laserson

using Lidemia.Core.Enums;
using Lidemia.Core.Models.Base;
using System.Text.Json.Serialization;

namespace Lidemia.Core.Models.Domain;

public class ProductModel : AbstractEntity<long>
{
    public string? Title { get; set; }

    [JsonIgnore]
    public long SupplierId { get; set; }

    public long? ParentId { get; set; }

    public string[] Tags { get; set; } = [];

    public string? Description { get; set; }

    public string? ShortDescription { get; set; }

    public int TotalRates { get; set; }

    public double? AverageRate { get; set; }

    public double? AverageRecentRate { get; set; }

    public decimal Price { get; set; }

    public ProductPriceUnit? PriceUnit { get; set; }

    public ProductAvailabilityType? AvailabilityType { get; set; }

    public decimal? MinQuantity { get; set; }

    public decimal? MaxQuantity { get; set; }

    public string? Sku { get; set; }

    public decimal? DiscountPrice { get; set; }

    public bool HasCurrentDiscount { get; set; }

    public DateTimeOffset? DiscountStartDate { get; set; }

    public DateTimeOffset? DiscountEndDate { get; set; }

    public SupplierModel Supplier { get; set; } = null!;

    public ProductCategoryModel? Category { get; set; }

    public PhotoModel PrimaryPhoto { get; set; } = null!;

    public string? Slug { get; set; }
}