// Created on 19/10/2026 20:18 by Laserson

namespace Lidemia.Core.Models.Dto;

public record AddToCartRequestModel
(
    long ProductId,
    double Qty
);