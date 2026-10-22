// Created on 02/09/2026 19:59 by Laserson

using Lidemia.Core.Models.Base;
using System.Text.Json.Serialization;

namespace Lidemia.Core.Models.Domain;

public class ProductModel : AbstractEntity<long>
{
    public string? Title { get; set; }

    [JsonIgnore]
    public long SupplierId { get; set; }

    public string[] Tags { get; set; } = [];

    public string? Description { get; set; }

    public int TotalRates { get; set; }

    public double AverageRate { get; set; }

    public decimal Price { get; set; }

    public decimal? DiscountPrice { get; set; }

    public bool HasCurrentDiscount { get; set; }

    public SupplierModel Supplier { get; set; } = null!;
}