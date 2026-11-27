// Created on 20/11/2026 19:21 by Laserson

using Lidemia.Core.Enums;
using Lidemia.Core.Models.Domain;

namespace Lidemia.ViewModels;

public record SupplierDetailsViewModel
(
    int TotalOrders,
    int TotalProducts,
    double AverageRate,
    SupplierFlags[] Flags,
    SupplierModel Supplier
);