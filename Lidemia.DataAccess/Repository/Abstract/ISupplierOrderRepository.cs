// Created on 04/11/2026 14:01 by Laserson

using FluentResults;
using Lidemia.Core.Models.Dto;
using Lidemia.Core.Models.Misc;
using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Repository.Abstract.Base;
using X.PagedList;

namespace Lidemia.DataAccess.Repository.Abstract;

public interface ISupplierOrderRepository : IRepository<SupplierOrderEntity, long>
{
    Task<Result<long>> Create(long supplierId, long? supplierAddressId, long? customerAddressId, IEnumerable<(long ProductId, int Quantity, Price PriceForSingle, Price? DiscountPriceForSingle)> items,
        long orderId, CancellationToken cancellationToken);

    Task<IPagedList<SupplierOrderEntity>> GetSupplierOrders(long supplierId, SupplierOrdersFilterModel filter, CancellationToken cancellationToken);

    Task<SupplierOrderEntity?> GetSupplierOrder(long id, long supplierId, CancellationToken cancellationToken);

    Task<IReadOnlyList<SupplierOrderEntity>> GetCustomerSupplierOrders(long customerId, long supplierId, int limit,
        CancellationToken cancellationToken);

    Task<SupplierOrderEntity?> GetCustomerSupplierOrder(long orderId, long customerId, long supplierId,
        CancellationToken cancellationToken);
}