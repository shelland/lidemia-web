// Created on 19/10/2026 19:37 by Laserson

using Lidemia.Core.Models.Domain;

namespace Lidemia.Core.Models.Misc;

public record ShoppingCartItem
(
    ProductModel Product,
    double Qty
);