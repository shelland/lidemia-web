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
            Items = model.Items.Select(item => new OrderItemEntity
            {
                ProductId = item.ProductId,
                Quantity = item.Qty
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
            .Where(x => x.ProductId == productId && x.Order.CustomerId == customerId)
            .OrderByDescending(x => x.Order.CreateDate)
            .Select(x => x.Order)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        return order;
    }

    public Task<OrderEntity?> GetCustomerOrder(long orderId, long customerId, CancellationToken cancellationToken)
    {
        return this.context.Orders
            .AsActive()
            .Include(x => x.Customer)
            .ThenInclude(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == orderId && x.CustomerId == customerId, cancellationToken);
    }
}