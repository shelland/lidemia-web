// Created on 19/10/2026 22:16 by Laserson

using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Core.Models.Dto;
using Lidemia.DataAccess.Repository.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.Common.BusinessLogic.Services.Data;

[ServiceDescriptor<IWishListService>(ServiceLifetime.Scoped)]
public class WishListService : IWishListService
{
    private readonly IWishListRepository wishListRepository;

    public WishListService(IWishListRepository wishListRepository)
    {
        this.wishListRepository = wishListRepository;
    }

    public Task<bool> AddToWishList(long customerId, AddToWishListRequestDto request, CancellationToken cancellationToken)
    {
        return this.wishListRepository.AddItem(customerId, request.ProductId, request.WishListId, cancellationToken);
    }
}