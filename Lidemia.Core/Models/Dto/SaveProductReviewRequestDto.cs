// Created on 30/10/2026 20:15 by Laserson

namespace Lidemia.Core.Models.Dto;

public record SaveProductReviewRequestDto
(
    long? Id,
    long ProductId,
    string Review,
    string? PositiveReview,
    string? NegativeReview,
    byte Rate,
    bool IsAnonymous
);