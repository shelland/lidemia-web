// Created on 03/09/2026 18:56 by Laserson

using Lidemia.Core.Models.Dto;
using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Repository.Abstract.Base;
using X.PagedList;

namespace Lidemia.DataAccess.Repository.Abstract;

public interface IWishListItemRepository : IRepository<WishListItemEntity, long>
{
    Task<int> CountUserLists(long customerId, CancellationToken cancellationToken);

    Task AddItem(long customerId, long productId, CancellationToken cancellationToken);

    Task<IPagedList<WishListItemEntity>> GetCustomerWishList(long customerId, CustomerWishListFilterModel filter, CancellationToken cancellationToken);
}