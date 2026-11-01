// Created on 31/10/2026 14:03 by Laserson

namespace Lidemia.Core.Models.Service;

public record SaveProductReviewModel
(
    long? Id,
    long CustomerId,
    long ProductId,
    string Review,
    string? PositiveReview,
    string? NegativeReview,
    byte Rate,
    bool IsAnonymous
);