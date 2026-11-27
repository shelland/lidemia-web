// Created on 01/09/2026 15:50 by Laserson

using Lidemia.Core.Enums;
using Lidemia.Core.Models.Base;
using Lidemia.Core.Models.Misc;

namespace Lidemia.Core.Models.Domain;

public class OrderModel : AbstractEntity<long>, IHasId<long>
{
    public CustomerModel? Customer { get; set; }

    public string Number { get; set; } = string.Empty;

    public Price TotalSum { get; set; } = null!;

    public Price? TotalSumDiscount { get; set; }

    public IEnumerable<OrderItemModel>? Items { get; set; } = [];
}

public record OrderItemModel
(
    ProductModel? Product,
    int Quantity,
    decimal PricePerItem,
    decimal? PricePerItemDiscount,
    DiscountSource? DiscountSource
);