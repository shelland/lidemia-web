// Created on 20/11/2026 17:23 by Laserson

namespace Lidemia.Core.Models.Domain;

public record SupplierStatsModel
(
    int TotalOrders,
    double AverageRating,
    int TotalProducts
);