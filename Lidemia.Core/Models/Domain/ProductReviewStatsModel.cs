// Created on 16/11/2026 19:01 by Laserson

namespace Lidemia.Core.Models.Domain;

public record ProductReviewStatsModel
(
    double? AverageRatingRecent,
    double? AverageRatingOverall,
    int TotalRates,
    IDictionary<byte, int> Distribution
);