// Created on 01/09/2026 15:44 by Laserson

using FluentResults;
using Lidemia.Core.Models.Misc;
using Lidemia.Core.Models.Service;
using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Repository.Abstract.Base;
using X.PagedList;

namespace Lidemia.DataAccess.Repository.Abstract;

public interface IOrderRepository : IRepository<OrderEntity, long>
{
    Task<IPagedList<OrderEntity>> GetCustomerOrders(long customerId, PagingInfoModel pagingInfo, CancellationToken cancellationToken);

    Task<Result<long>> Create(long customerId, string number, CreateOrderModel model, CancellationToken cancellationToken);

    Task<OrderEntity?> GetUserProductLastOrder(long customerId, long productId, CancellationToken cancellationToken);

    Task<OrderEntity?> GetCustomerOrder(long orderId, long customerId, CancellationToken cancellationToken);
}