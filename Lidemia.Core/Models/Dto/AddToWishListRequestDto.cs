// Created on 19/10/2026 22:11 by Laserson

namespace Lidemia.Core.Models.Dto;

public record AddToWishListRequestDto
(
    long ProductId,
    Guid? WishListId
);