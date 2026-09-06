// Created on 03/09/2026 18:57 by Laserson

using Lidemia.DataAccess.Context;
using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Repository.Abstract;
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
}