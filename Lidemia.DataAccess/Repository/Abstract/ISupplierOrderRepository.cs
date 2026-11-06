// Created on 04/11/2026 14:01 by Laserson

using FluentResults;
using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Repository.Abstract.Base;

namespace Lidemia.DataAccess.Repository.Abstract;

public interface ISupplierOrderRepository : IRepository<SupplierOrderEntity, long>
{
    Task<Result<long>> Create(long supplierId, long productId, long orderId, double qty, CancellationToken cancellationToken);
}