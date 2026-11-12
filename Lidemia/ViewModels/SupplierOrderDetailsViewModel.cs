// Created on 12/11/2026 13:42 by Laserson

using Lidemia.Core.Models.Domain;

namespace Lidemia.ViewModels;

public record SupplierOrderDetailsViewModel
(
    SupplierOrderModel Order,
    IEnumerable<ProductModel> Products
);