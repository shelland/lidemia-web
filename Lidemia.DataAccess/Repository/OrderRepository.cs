// Created on 01/09/2026 15:46 by Laserson

using FluentResults;
using Lidemia.Core.Enums;
using Lidemia.Core.Extensions;
using Lidemia.Core.Models.Misc;
using Lidemia.Core.Models.Service;
using Lidemia.DataAccess.Context;
using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Extensions;
using Lidemia.DataAccess.Repository.Abstract;
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
        return this.context.Orders.AsActive()
            .Where(x => x.CustomerId == customerId)
            .OrderByDescending(x => x.Id)
            .ToPagedListEx(pagingInfo, cancellationToken);
    }

    public async Task<Result<long>> Create(string number, CreateOrderModel model, CancellationToken cancellationToken)
    {
        var entity = new OrderEntity
        {
            CustomerId = model.CustomerId,
            Comment = model.Comment,
            Number = number,
            Items = model.Items.Select(item => new OrderItemEntity
            {
                ProductId = item.ProductId,
                Quantity = item.Qty
            }),
            PromoCodeId = model.PromoCodeId,
            Status = OrderStatus.New,
        };

        this.context.Orders.Add(entity);
        await this.context.SaveChangesAsync(cancellationToken);

        return Result.Ok(entity.Id);
    }
}