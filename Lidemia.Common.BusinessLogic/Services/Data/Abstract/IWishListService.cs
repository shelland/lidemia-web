// Created on 02/09/2026 20:09 by Laserson

using Lidemia.Core.Models.Domain;
using Lidemia.Core.Models.Dto;
using Lidemia.Core.Models.Misc;

namespace Lidemia.Common.BusinessLogic.Services.Data.Abstract;

public interface IWishListService
{
    Task AddToWishList(long customerId, AddToWishListRequestDto request, CancellationToken cancellationToken);

    Task<BasePagedListModel<WishListItemModel>> GetCustomerWishList(long customerId, CustomerWishListFilterModel filter, CancellationToken cancellationToken);
}