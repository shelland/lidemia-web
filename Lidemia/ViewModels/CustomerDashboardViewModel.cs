// Created on 26/11/2026 15:44 by Laserson

using Lidemia.Core.Models.Domain;

namespace Lidemia.ViewModels;

public record CustomerDashboardViewModel
(
    IEnumerable<ProductModel> ViewedProducts
);