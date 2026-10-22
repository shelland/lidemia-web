// Created on 03/09/2026 18:57 by Laserson

using Lidemia.Core.Extensions;
using Lidemia.DataAccess.Context;
using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Extensions;
using Lidemia.DataAccess.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.DataAccess.Repository;

[ServiceDescriptor<IWishListRepository>(ServiceLifetime.Scoped)]
public class WishListRepository : IWishListRepository
{
    private readonly LidemiaDbContext context;

    public WishListRepository(LidemiaDbContext context)
    {
        this.context = context;
    }

    public Task<WishListEntity?> GetById(long key, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Delete(long key, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<int> CountUserLists(long customerId, CancellationToken cancellationToken)
    {
        return this.context.WishLists.AsActive().CountAsync(x => x.CustomerId == customerId, cancellationToken: cancellationToken);
    }

    public async Task<bool> AddItem(long customerId, long productId, long? wishListId, CancellationToken cancellationToken)
    {
        var strategy = this.context.Database.CreateExecutionStrategy();

        return await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await this.context.Database.BeginTransactionAsync(cancellationToken);

            WishListEntity targetList;

            if (wishListId == null)
            {
                var defaultList = await this.context.WishLists
                    .AsActive()
                    .FirstOrDefaultAsync(x => x.CustomerId == customerId, cancellationToken: cancellationToken);

                if (defaultList == null)
                {
                    targetList = new WishListEntity
                    {
                        CustomerId = customerId,
                        ItemsCount = 0,
                    };

                    this.context.WishLists.Add(targetList);
                }
                else
                {
                    targetList = defaultList;
                }
            }
            else
            {
                targetList = (await this.context.WishLists
                    .AsActive()
                    .FirstOrDefaultAsync(x => x.Id == wishListId, cancellationToken: cancellationToken)).NotNull();

                var existingEntry = await this.context.WishListItems.FirstOrDefaultAsync(x => x.ProductId == productId && x.WishListId == wishListId,
                    cancellationToken: cancellationToken);

                if (existingEntry != null)
                {
                    return false;
                }
            }

            var listItemsCount = await this.context.WishListItems
                .AsActive()
                .CountAsync(x => x.WishListId == targetList.Id, cancellationToken: cancellationToken) + 1;

            await this.context.WishLists
                .AsActive()
                .Where(x => x.Id == targetList.Id)
                .ExecuteUpdateAsync(x => x.SetProperty(e => e.ItemsCount, listItemsCount), cancellationToken: cancellationToken);

            await transaction.CommitAsync(cancellationToken);
            return true;
        });
    }
}