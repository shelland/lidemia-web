// Created on 15/11/2026 14:24 by Laserson

namespace Lidemia.DataAccess.Entities.Misc;

public record ProductReviewRateDistributionDbModel
(
    IDictionary<byte, int> Distribution
);