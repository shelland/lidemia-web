// Created on 04/11/2026 14:01 by Laserson

using FluentResults;
using Lidemia.Core.Enums;
using Lidemia.Core.Extensions;
using Lidemia.Core.Models.Dto;
using Lidemia.Core.Models.Misc;
using Lidemia.DataAccess.Context;
using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Extensions;
using Lidemia.DataAccess.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using X.PagedList;

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
        return this.context.SupplierOrders.ExecuteUpdateAsync(x => x.SetProperty(e => e.IsActive, false), cancellationToken);
    }

    public async Task<Result<long>> Create(long supplierId, long? supplierAddressId, long? customerAddressId,
        IEnumerable<(long ProductId, int Quantity, Price PriceForSingle, Price? DiscountPriceForSingle)> items,
        long orderId,
        CancellationToken cancellationToken)
    {
        var entity = new SupplierOrderEntity
        {
            SupplierId = supplierId,
            OrderId = orderId,
            Status = OrderStatus.New,
            CustomerAddressId = customerAddressId,
            SupplierAddressId = supplierAddressId,
            Items = new SupplierOrderItems(items.Select(x => new SupplierOrderItem(
                ProductId: x.ProductId,
                Quantity: x.Quantity,
                PriceForSingle: x.PriceForSingle.Value,
                DiscountPriceForSingle: x.DiscountPriceForSingle?.Value
            )).ToList()),
        };

        this.context.SupplierOrders.Add(entity);
        await this.context.SaveChangesAsync(cancellationToken);

        return Result.Ok(entity.Id);
    }

    public async Task<IPagedList<SupplierOrderEntity>> GetSupplierOrders(long supplierId, SupplierOrdersFilterModel filter, CancellationToken cancellationToken)
    {
        var orders = this.context.SupplierOrders.AsActive()
            .Include(x => x.Order)
            .Include(x => x.CustomerAddress)
            .Include(x => x.Order)
            .ThenInclude(x => x.Customer)
            .ThenInclude(x => x.User)
            .Where(x => x.SupplierId == supplierId);

        orders = filter.Status != null ? orders.Where(x => x.Status == filter.Status) : orders.Where(x => x.Status != OrderStatus.Finished);
        orders = orders.OrderByDescending(x => x.Id);

        return await orders.ToPagedListEx(filter, cancellationToken);
    }

    public Task<SupplierOrderEntity?> GetSupplierOrder(long id, long supplierId, CancellationToken cancellationToken)
    {
        return this.context.SupplierOrders.AsActive()
            .Include(x => x.Order)
            .ThenInclude(x => x.Customer)
            .ThenInclude(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id && x.SupplierId == supplierId, cancellationToken: cancellationToken);
    }
}