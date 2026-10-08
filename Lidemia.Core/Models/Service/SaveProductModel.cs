// Created on 14/09/2026 20:35 by Laserson

using Lidemia.Core.Enums;

namespace Lidemia.Core.Models.Service;

public record SaveProductModel
(
    long? Id,
    long? ParentId,
    string? Title,
    string? Description,
    string? ShortDescription,
    ProductAvailabilityType? AvailabilityType
);