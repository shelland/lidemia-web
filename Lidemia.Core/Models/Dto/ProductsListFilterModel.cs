// Created on 03/09/2026 20:32 by Laserson

using Lidemia.Core.Models.Misc;

namespace Lidemia.Core.Models.Dto;

public class ProductsListFilterModel : PagingInfoModel
{
    public long? CategoryId { get; set; }

    public HashSet<long>? Suppliers { get; set; }

    public decimal? PriceFrom { get; set; }

    public decimal? PriceTo { get; set; }

    public string? Title { get; set; }

    public string? Tag { get; set; }

    public bool HasSearchParams() =>
        CategoryId.HasValue ||
        Suppliers?.Count > 0 ||
        PriceFrom > 0 ||
        PriceTo > 0 ||
        !string.IsNullOrEmpty(Title) ||
        !string.IsNullOrEmpty(Tag);
}