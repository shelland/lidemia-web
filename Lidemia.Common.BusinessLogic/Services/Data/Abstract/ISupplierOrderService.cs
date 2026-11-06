// Created on 04/11/2026 14:23 by Laserson

using FluentResults;

namespace Lidemia.Common.BusinessLogic.Services.Data.Abstract;

public interface ISupplierOrderService
{
    Task<Result<long>> Create(long supplierId, long productId, long orderId, double qty, CancellationToken cancellationToken);
}