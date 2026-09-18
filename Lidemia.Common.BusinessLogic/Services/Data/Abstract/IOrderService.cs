// Created on 02/09/2026 20:09 by Laserson

using FluentResults;
using Lidemia.Core.Models.Domain;
using Lidemia.Core.Models.Misc;
using Lidemia.Core.Models.Service;

namespace Lidemia.Common.BusinessLogic.Services.Data.Abstract;

public interface IOrderService
{
    Task<Result<long>> Create(CreateOrderModel model, CancellationToken cancellationToken);

    Task<BasePagedListModel<OrderModel>> GetCustomerOrders(PagingInfoModel pagingInfo, long customerId, CancellationToken cancellationToken);
}