// Created on 08/10/2026 18:28 by Laserson

namespace Lidemia.Core.Models.Dto;

public record SetProductVisibilityRequestDto
(
    long ProductId,
    bool IsVisible
);