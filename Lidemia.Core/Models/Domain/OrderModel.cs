// Created on 01/09/2026 15:50 by Laserson

using Lidemia.Core.Models.Base;

namespace Lidemia.Core.Models.Domain;

public class OrderModel : AbstractEntity<long>, IHasId<long>
{
    public CustomerModel Customer { get; set; } = null!;

    public string Number { get; set; } = string.Empty;

    public IEnumerable<ProductItemModel> Items { get; set; } = [];
}

public record ProductItemModel
(
    ProductModel Product,
    double Quantity
);