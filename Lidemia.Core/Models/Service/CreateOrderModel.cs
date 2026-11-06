// Created on 07/09/2026 20:15 by Laserson

namespace Lidemia.Core.Models.Service;

public record CreateOrderModel
(
    long CustomerId,
    string? Comment,
    IEnumerable<CreateOrderItemModel> Items
);

public record CreateOrderItemModel
(
    long ProductId,
    double Qty,
    long? PromoCodeId
);