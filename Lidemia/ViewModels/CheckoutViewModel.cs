// Created on 03/11/2026 18:23 by Laserson

using Lidemia.Core.Models.Domain;

namespace Lidemia.ViewModels;

public record CheckoutViewModel
(
    decimal CartTotal,
    IDictionary<(long SupplierId, string SupplierName), IEnumerable<(ProductModel Product, int Qty)>> Items
);