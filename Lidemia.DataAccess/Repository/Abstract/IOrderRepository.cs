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

    Task<Result<long>> Create(string number, CreateOrderModel model, CancellationToken cancellationToken);
}