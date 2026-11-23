// Created on 01/09/2026 15:46 by Laserson

using FluentResults;
using Lidemia.Core.Extensions;
using Lidemia.Core.Models.Misc;
using Lidemia.Core.Models.Service;
using Lidemia.DataAccess.Context;
using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Entities.Meta;
using Lidemia.DataAccess.Extensions;
using Lidemia.DataAccess.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using X.PagedList;

namespace Lidemia.DataAccess.Repository;

[ServiceDescriptor<IOrderRepository>(ServiceLifetime.Scoped)]
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

    public Task<IPagedList<OrderEntity>> GetCustomerOrders(long customerId, PagingInfoModel pagingInfo, CancellationToken cancellationToken)
    {
        return this.context.Orders
            .AsActive()
            .Include(x => x.Customer)
            .ThenInclude(x => x.User)
            .Where(x => x.CustomerId == customerId)
            .OrderByDescending(x => x.Id)
            .ToPagedListEx(pagingInfo, cancellationToken);
    }

    public async Task<Result<long>> Create(long customerId, string number, CreateOrderModel model, CancellationToken cancellationToken)
    {
        var entity = new OrderEntity
        {
            CustomerId = customerId,
            Comment = model.Comment,
            Number = number,
            Metadata = OrderMetadata.Default,
            TotalSum = model.TotalSum,
            TotalSumDiscount = model.TotalSumDiscount,
            Items = model.Items.Select(item => new OrderItemEntity
            {
                ProductId = item.ProductId,
                Quantity = item.Qty,
                PricePerItem = item.PricePerItem.Value,
                PricePerItemDiscount = item.PricePerItemDiscount?.Value,
                DiscountSource = item.DiscountSource,
            }).ToList(),
        };

        this.context.Orders.Add(entity);
        await this.context.SaveChangesAsync(cancellationToken);

        return Result.Ok(entity.Id);
    }

    public Task<OrderEntity?> GetUserProductLastOrder(long customerId, long productId, CancellationToken cancellationToken)
    {
        var order = this.context.OrderItems
            .AsActive()
            .Include(x => x.Order).ThenInclude(x => x.Customer).ThenInclude(x => x.User)
            .Where(x => x.ProductId == productId && x.Order.CustomerId == customerId)
            .OrderByDescending(x => x.Order.CreateDate)
            .Select(x => x.Order)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        return order;
    }

    public async Task<OrderEntity?> GetCustomerOrder(long orderId, long customerId, CancellationToken cancellationToken)
    {
        var items = await this.context.OrderItems
            .Where(x => x.OrderId == orderId)
            .Include(x => x.Product)
            .ThenInclude(x => x.Supplier)
            .ThenInclude(x => x.User)
            .ToListAsync(cancellationToken);

        var order = await this.context.Orders
            .AsActive()
            .Include(x => x.Customer)
            .ThenInclude(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == orderId && x.CustomerId == customerId, cancellationToken);

        if (order == null)
        {
            return null;
        }

        order.Items = items;
        return order;
    }
}