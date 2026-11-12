// Created on 04/11/2026 14:23 by Laserson

using FluentResults;
using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Common.Mapper;
using Lidemia.Common.Mapper.Abstract;
using Lidemia.Core.Models.Domain;
using Lidemia.Core.Models.Dto;
using Lidemia.Core.Models.Misc;
using Lidemia.DataAccess.Repository.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.Common.BusinessLogic.Services.Data;

[ServiceDescriptor<ISupplierOrderService>(ServiceLifetime.Scoped)]
public class SupplierOrderService : ISupplierOrderService
{
    private readonly ISupplierOrderRepository supplierOrderRepository;
    private readonly ISupplierOrderMapper supplierOrderMapper;

    public SupplierOrderService(ISupplierOrderRepository supplierOrderRepository, ISupplierOrderMapper supplierOrderMapper)
    {
        this.supplierOrderRepository = supplierOrderRepository;
        this.supplierOrderMapper = supplierOrderMapper;
    }

    public Task<Result<long>> Create(long supplierId, long? supplierAddressId, long? customerAddressId, IEnumerable<(long ProductId, int Quantity, decimal PriceForSingle, decimal? DiscountPriceForSingle)> items,
        long orderId, CancellationToken cancellationToken)
    {
        return this.supplierOrderRepository.Create(supplierId, supplierAddressId, customerAddressId, items, orderId, cancellationToken);
    }

    public async Task<BasePagedListModel<SupplierOrderModel>> GetSupplierOrders(long supplierId, SupplierOrdersFilterModel filter,
        CancellationToken cancellationToken)
    {
        var orders = await this.supplierOrderRepository.GetSupplierOrders(supplierId, filter, cancellationToken);

        return new BasePagedListModel<SupplierOrderModel>
        {
            CurrentPage = filter.Page,
            TotalCount = orders.TotalItemCount,
            TotalPages = orders.PageCount,
            Items = this.supplierOrderMapper.MapList(orders)
        };
    }

    public async Task<SupplierOrderModel?> GetSupplierOrder(long id, long supplierId, CancellationToken cancellationToken)
    {
        var order = await this.supplierOrderRepository.GetSupplierOrder(id, supplierId, cancellationToken);
        return order != null ? this.supplierOrderMapper.Map(order) : null;
    }
}