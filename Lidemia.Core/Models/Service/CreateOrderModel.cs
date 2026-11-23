// Created on 07/09/2026 20:15 by Laserson

using Lidemia.Core.Enums;
using Lidemia.Core.Models.Misc;

namespace Lidemia.Core.Models.Service;

public record CreateOrderModel
(
    string? Comment,
    long? CustomerAddressId,
    decimal TotalSum,
    decimal? TotalSumDiscount,
    IEnumerable<CreateOrderItemModel> Items
);

public record CreateOrderItemModel
(
    long ProductId,
    string? PromoCode,
    long? SupplierAddressId,
    int Qty,
    Price PricePerItem,
    Price? PricePerItemDiscount,
    DiscountSource? DiscountSource
);

public record CreateOrderServiceModel
(
    string? Comment,
    long? CustomerAddressId,
    IEnumerable<CreateOrderServiceModelItem> Items
);

public record CreateOrderServiceModelItem
(
    string? PromoCode,
    long? SupplierAddressId,
    long SupplierId
);