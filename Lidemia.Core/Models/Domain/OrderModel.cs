// Created on 01/09/2026 15:50 by Laserson

using Lidemia.Core.Enums;
using Lidemia.Core.Models.Base;

namespace Lidemia.Core.Models.Domain;

public class OrderModel : IHasId<long>
{
    public long Id { get; set; }

    public DateTimeOffset Date { get; set; }

    public CustomerModel Customer { get; set; } = null!;

    public string Number { get; set; } = string.Empty;

    public OrderDeliveryType DeliveryType { get; set; }

    public IEnumerable<ProductItemModel> Items { get; set; } = [];
}

public record ProductItemModel
(
    ProductModel Product,
    double Quantity
);