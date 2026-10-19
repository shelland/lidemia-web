// Created on 03/09/2026 18:57 by Laserson

using Lidemia.DataAccess.Context;
using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Entities.Misc;
using Lidemia.DataAccess.Extensions;
using Lidemia.DataAccess.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.DataAccess.Repository;

[ServiceDescriptor<IShoppingCartRepository>(ServiceLifetime.Scoped)]
public class ShoppingCartRepository : IShoppingCartRepository
{
    private readonly LidemiaDbContext context;

    public ShoppingCartRepository(LidemiaDbContext context)
    {
        this.context = context;
    }

    public Task<ShoppingCartEntity?> GetById(Guid key, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Delete(Guid key, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<ShoppingCartEntity?> GetCustomerShoppingCart(long customerId, CancellationToken cancellationToken)
    {
        return this.context.ShoppingCarts.AsActive().FirstOrDefaultAsync(x => x.CustomerId == customerId, cancellationToken);
    }

    public async Task AddProduct(long customerId, long productId, double qty, CancellationToken cancellationToken)
    {
        var cart = await this.context.ShoppingCarts.AsActive().Include(shoppingCartEntity => shoppingCartEntity.Items)
            .FirstOrDefaultAsync(x => x.CustomerId == customerId, cancellationToken);

        if (cart == null)
        {
            cart = new ShoppingCartEntity
            {
                CustomerId = customerId,
                Items = [new ShoppingCartItemDbModel(ProductId: productId, Qty: qty)]
            };

            this.context.ShoppingCarts.Add(cart);
            await this.context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            var currentItems = cart.Items.ToDictionary(x => x.ProductId, x => x.Qty);

            if (currentItems.TryGetValue(productId, out var existingItem))
            {
                currentItems[productId] = existingItem + qty;
            }
            else
            {
                currentItems[productId] = qty;
            }

            var newDbItems = currentItems.Select(x => new ShoppingCartItemDbModel(ProductId: x.Key, Qty: x.Value));

            await this.context.ShoppingCarts.AsActive()
                .Where(x => x.CustomerId == customerId)
                .ExecuteUpdateAsync(x => x.SetProperty(e => e.Items, newDbItems), cancellationToken: cancellationToken);
        }
    }
}