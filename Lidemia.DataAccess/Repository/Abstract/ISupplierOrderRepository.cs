// Created on 04/11/2026 14:01 by Laserson

using FluentResults;
using Lidemia.Core.Models.Dto;
using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Repository.Abstract.Base;
using X.PagedList;

namespace Lidemia.DataAccess.Repository.Abstract;

public interface ISupplierOrderRepository : IRepository<SupplierOrderEntity, long>
{
    Task<Result<long>> Create(long supplierId, long? supplierAddressId, long? customerAddressId, IEnumerable<(long ProductId, int Quantity, decimal PriceForSingle, decimal? DiscountPriceForSingle)> items,
        long orderId, CancellationToken cancellationToken);

    Task<IPagedList<SupplierOrderEntity>> GetSupplierOrders(long supplierId, SupplierOrdersFilterModel filter, CancellationToken cancellationToken);

    Task<SupplierOrderEntity?> GetSupplierOrder(long id, long supplierId, CancellationToken cancellationToken);
}