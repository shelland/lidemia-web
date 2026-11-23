// Created on 04/11/2026 14:23 by Laserson

using FluentResults;
using Lidemia.Core.Models.Domain;
using Lidemia.Core.Models.Dto;
using Lidemia.Core.Models.Misc;

namespace Lidemia.Common.BusinessLogic.Services.Data.Abstract;

public interface ISupplierOrderService
{
    Task<Result<long>> Create(long supplierId, long? supplierAddressId, long? customerAddressId,
        IEnumerable<(long ProductId, int Quantity, Price PriceForSingle, Price? DiscountPriceForSingle)> items,
        long orderId, CancellationToken cancellationToken);

    Task<BasePagedListModel<SupplierOrderModel>> GetSupplierOrders(long supplierId, SupplierOrdersFilterModel filter, CancellationToken cancellationToken);

    Task<SupplierOrderModel?> GetSupplierOrder(long id, long supplierId, CancellationToken cancellationToken);

    Task<IReadOnlyList<SupplierOrderModel>> GetCustomerSupplierOrders(long customerId, long supplierId, CancellationToken cancellationToken);

    Task<SupplierOrderModel?> GetCustomerSupplierOrder(long orderId, long customerId, long supplierId, CancellationToken cancellationToken);
}
