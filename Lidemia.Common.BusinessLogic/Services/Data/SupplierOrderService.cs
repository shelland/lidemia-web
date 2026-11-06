// Created on 04/11/2026 14:23 by Laserson

using FluentResults;
using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.DataAccess.Repository.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.Common.BusinessLogic.Services.Data;

[ServiceDescriptor<ISupplierOrderService>(ServiceLifetime.Scoped)]
public class SupplierOrderService : ISupplierOrderService
{
    private readonly ISupplierOrderRepository supplierOrderRepository;

    public SupplierOrderService(ISupplierOrderRepository supplierOrderRepository)
    {
        this.supplierOrderRepository = supplierOrderRepository;
    }

    public Task<Result<long>> Create(long supplierId, long productId, long orderId, double qty, CancellationToken cancellationToken)
    {
        return this.supplierOrderRepository.Create(supplierId, productId, orderId, qty, cancellationToken);
    }
}