// Created on 03/09/2026 20:34 by Laserson

using Lidemia.Core.Models.Domain;
using Lidemia.Core.Models.Dto;

namespace Lidemia.ViewModels;

public class ProductsListViewModel : BaseListViewModel<ProductModel, ProductsListFilterModel>
{
    public IDictionary<long, decimal>? CartItems { get; set; }

    public IEnumerable<ProductCategoryModel> Categories { get; set; } = null!;
}