// Created on 30/09/2026 18:26 by Laserson

using Lidemia.Core.Enums;

namespace Lidemia.Core.Models.Dto;

public record SaveProductRequestDto
(
    long? Id,
    long? ParentId,
    string? Title,
    string? Description,
    string? ShortDescription,
    ProductAvailabilityType? AvailabilityType,
    bool? HasCurrentDiscount,
    double? DiscountPrice,
    DateTime? DiscountStartDate,
    DateTime? DiscountEndDate,
    double? MinQuantity,
    double? MaxQuantity
);