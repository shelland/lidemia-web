// Created on 02/09/2026 20:09 by Laserson

using Lidemia.Core.Models.Dto;

namespace Lidemia.Common.BusinessLogic.Services.Data.Abstract;

public interface IWishListService
{
    Task<bool> AddToWishList(long customerId, AddToWishListRequestDto request, CancellationToken cancellationToken);
}