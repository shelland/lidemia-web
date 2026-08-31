// Created on 01/09/2026 15:46 by Laserson

using Lidemia.DataAccess.Context;
using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Repository.Abstract;

namespace Lidemia.DataAccess.Repository;

public class OrderRepository : IOrderRepository
{
    private readonly LidemiaDbContext context;

    public OrderRepository(LidemiaDbContext context)
    {
        this.context = context;
    }

    public Task<OrderEntity?> GetById(long key, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Delete(long key, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}