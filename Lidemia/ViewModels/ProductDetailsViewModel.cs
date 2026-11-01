// Created on 04/09/2026 18:24 by Laserson

using Lidemia.Core.Models.Domain;
using Lidemia.Core.Models.Misc;

namespace Lidemia.ViewModels;

public record ProductDetailsViewModel
(
    ProductModel Product,
    OrderModel? CurrentUserOrder,
    ProductReviewModel? CurrentUserReview,
    IReadOnlyList<PhotoModel> Photos,
    IReadOnlyList<LightProductModel> RelatedProducts,
    IReadOnlyList<ProductReviewModel> RecentReviews
);