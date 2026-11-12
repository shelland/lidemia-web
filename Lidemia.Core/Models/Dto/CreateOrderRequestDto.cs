// Created on 03/11/2026 18:43 by Laserson

namespace Lidemia.Core.Models.Dto;

public record CreateOrderRequestDto
(
    IEnumerable<OrderItem> Items,
    string? Comment,
    long? CustomerAddressId
);

public record OrderItem
(
    long SupplierId,
    long? SupplierAddressId,
    string? PromoCode
);