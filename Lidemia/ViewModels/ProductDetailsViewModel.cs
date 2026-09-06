// Created on 04/09/2026 18:24 by Laserson

using Lidemia.Core.Models.Domain;

namespace Lidemia.ViewModels;

public record ProductDetailsViewModel
(
    ProductModel Product,
    IEnumerable<PhotoModel> Photos
);