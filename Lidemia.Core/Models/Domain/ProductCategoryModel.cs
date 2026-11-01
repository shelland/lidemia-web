// Created on 23/10/2026 19:17 by Laserson

using Lidemia.Core.Models.Base;

namespace Lidemia.Core.Models.Domain;

public class ProductCategoryModel : AbstractEntity<long>
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public ProductCategoryModel? Parent { get; set; }

    public List<ProductCategoryModel> Children { get; set; } = [];
}