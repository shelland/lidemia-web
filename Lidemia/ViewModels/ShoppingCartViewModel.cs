// Created on 19/10/2026 20:13 by Laserson

using Lidemia.Core.Models.Misc;

namespace Lidemia.ViewModels;

public record ShoppingCartViewModel
(
    IEnumerable<ShoppingCartItem>? Items
);