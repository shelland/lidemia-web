// Created on 04/11/2026 14:01 by Laserson

using FluentResults;
using Lidemia.Core.Enums;
using Lidemia.DataAccess.Context;
using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Repository.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.DataAccess.Repository;

[ServiceDescriptor<ISupplierOrderRepository>(ServiceLifetime.Scoped)]
public class SupplierOrderRepository : ISupplierOrderRepository
{
    private readonly LidemiaDbContext context;

    public SupplierOrderRepository(LidemiaDbContext context)
    {
        this.context = context;
    }

    public Task<SupplierOrderEntity?> GetById(long key, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Delete(long key, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<long>> Create(long supplierId, long productId, long orderId, double qty, CancellationToken cancellationToken)
    {
        var entity = new SupplierOrderEntity
        {
            SupplierId = supplierId,
            ProductId = productId,
            OrderId = orderId,
            Qty = qty,
            Status = OrderStatus.New
        };

        this.context.SupplierOrders.Add(entity);
        await this.context.SaveChangesAsync(cancellationToken);

        return Result.Ok(entity.Id);
    }
}