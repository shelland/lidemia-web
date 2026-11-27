// Created on 03/09/2026 18:57 by Laserson

using Lidemia.Core.Extensions;
using Lidemia.Core.Models.Dto;
using Lidemia.DataAccess.Context;
using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Extensions;
using Lidemia.DataAccess.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using X.PagedList;

namespace Lidemia.DataAccess.Repository;

[ServiceDescriptor<IWishListItemRepository>(ServiceLifetime.Scoped)]
public class WishListItemRepository : IWishListItemRepository
{
    private readonly LidemiaDbContext context;

    public WishListItemRepository(LidemiaDbContext context)
    {
        this.context = context;
    }

    public Task<WishListItemEntity?> GetById(long key, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Delete(long key, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<int> CountUserLists(long customerId, CancellationToken cancellationToken)
    {
        return this.context.WishListItems.AsActive().CountAsync(x => x.CustomerId == customerId, cancellationToken: cancellationToken);
    }

    public async Task AddItem(long customerId, long productId, CancellationToken cancellationToken)
    {
        var existing = await this.context
            .WishListItems
            .FirstOrDefaultAsync(x => x.CustomerId == customerId && x.ProductId == productId, cancellationToken: cancellationToken);

        if (existing is { IsActive: false })
        {
            await this.context.WishListItems.ExecuteUpdateAsync(x => x.SetProperty(e => e.IsActive, true), cancellationToken: cancellationToken);
        }
        else
        {
            this.context.WishListItems.Add(new WishListItemEntity
            {
                CustomerId = customerId,
                ProductId = productId
            });

            await this.context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<IPagedList<WishListItemEntity>> GetCustomerWishList(long customerId, CustomerWishListFilterModel filter,
        CancellationToken cancellationToken)
    {
        return await this.context.WishListItems
            .AsActive()
            .Include(x => x.Product)
            .OrderByDescending(x => x.Id)
            .ToPagedListEx(filter, cancellationToken);
    }
}