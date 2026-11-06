// Created on 03/11/2026 18:43 by Laserson

namespace Lidemia.Core.Models.Dto;

public record CreateOrderRequestDto
(
    IEnumerable<OrderItem> Items,
    string? Comment,
    long? PromoCodeId
);

public record OrderItem
(
    long ProductId,
    double Qty
);