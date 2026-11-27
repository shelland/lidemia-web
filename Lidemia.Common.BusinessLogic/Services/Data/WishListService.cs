// Created on 19/10/2026 22:16 by Laserson

using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Common.Mapper;
using Lidemia.Common.Mapper.Abstract;
using Lidemia.Core.Extensions;
using Lidemia.Core.Models.Domain;
using Lidemia.Core.Models.Dto;
using Lidemia.Core.Models.Misc;
using Lidemia.DataAccess.Repository.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.Common.BusinessLogic.Services.Data;

[ServiceDescriptor<IWishListService>(ServiceLifetime.Scoped)]
public class WishListService : IWishListService
{
    private readonly IWishListItemRepository wishListItemRepository;
    private readonly IWishListItemMapper mapper;

    public WishListService(IWishListItemRepository wishListItemRepository, IWishListItemMapper mapper)
    {
        this.wishListItemRepository = wishListItemRepository;
        this.mapper = mapper;
    }

    public Task AddToWishList(long customerId, AddToWishListRequestDto request, CancellationToken cancellationToken)
    {
        return this.wishListItemRepository.AddItem(customerId, request.ProductId, cancellationToken);
    }

    public async Task<BasePagedListModel<WishListItemModel>> GetCustomerWishList(long customerId, CustomerWishListFilterModel filter,
        CancellationToken cancellationToken)
    {
        var items = await this.wishListItemRepository.GetCustomerWishList(customerId, filter, cancellationToken);

        return new BasePagedListModel<WishListItemModel>
        {
            CurrentPage = filter.Page,
            Items = this.mapper.MapList(items).NotNull(),
            TotalCount = items.TotalItemCount,
            TotalPages = items.PageCount
        };
    }
}