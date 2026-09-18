// Created on 03/09/2026 18:48 by Laserson

using FluentResults;
using Lidemia.Common.BusinessLogic.Services.App.Abstract;
using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Common.Mapping;
using Lidemia.Common.Metrics;
using Lidemia.Core.Models.Domain;
using Lidemia.Core.Models.Misc;
using Lidemia.Core.Models.Service;
using Lidemia.DataAccess.Repository.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Scrutor;

namespace Lidemia.Common.BusinessLogic.Services.Data;

[ServiceDescriptor<IOrderService>(ServiceLifetime.Scoped)]
public class OrderService : IOrderService
{
    private readonly IOrderRepository orderRepository;
    private readonly IOrderNumberGenerator orderNumberGenerator;
    private readonly OrderMetrics metrics;
    private readonly ILogger<OrderService> logger;

    public OrderService(IOrderRepository orderRepository, IOrderNumberGenerator orderNumberGenerator, OrderMetrics metrics, ILogger<OrderService> logger)
    {
        this.orderRepository = orderRepository;
        this.orderNumberGenerator = orderNumberGenerator;
        this.metrics = metrics;
        this.logger = logger;
    }

    public async Task<Result<long>> Create(CreateOrderModel model, CancellationToken cancellationToken)
    {
        var number = this.orderNumberGenerator.Generate();
        var result = await this.orderRepository.Create(number, model, cancellationToken);

        this.metrics.OnNewOrder();
        this.logger.LogInformation("A new order {Number} was created by {CustomerId}", number, model.CustomerId);

        return result;
    }

    public async Task<BasePagedListModel<OrderModel>> GetCustomerOrders(PagingInfoModel pagingInfo, long customerId, CancellationToken cancellationToken)
    {
        var orders = await this.orderRepository.GetCustomerOrders(customerId, pagingInfo, cancellationToken);

        return new BasePagedListModel<OrderModel>
        {
            CurrentPage = pagingInfo.Page,
            Items = orders.Select(x => x.ToModel()),
            TotalCount = orders.TotalItemCount,
            TotalPages = orders.PageCount
        };
    }
}