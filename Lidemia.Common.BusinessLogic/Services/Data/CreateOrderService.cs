// Created on 04/11/2026 14:21 by Laserson

using FluentResults;
using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Common.Metrics;
using Lidemia.Core.Models.Bus;
using Lidemia.Core.Models.Service;
using Lidemia.DataAccess.Context;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Scrutor;

namespace Lidemia.Common.BusinessLogic.Services.Data;

[ServiceDescriptor<ICreateOrderService>(ServiceLifetime.Scoped)]
public class CreateOrderService : ICreateOrderService
{
    private readonly LidemiaDbContext context;
    private readonly IOrderService orderService;
    private readonly ISupplierOrderService supplierOrderService;
    private readonly IPublishEndpoint publishEndpoint;
    private readonly IProductService productService;
    private readonly OrderMetrics metrics;
    private readonly ILogger<CreateOrderService> logger;

    public CreateOrderService(
        LidemiaDbContext context,
        IOrderService orderService,
        ISupplierOrderService supplierOrderService,
        IPublishEndpoint publishEndpoint,
        IProductService productService,
        OrderMetrics metrics,
        ILogger<CreateOrderService> logger)
    {
        this.context = context;
        this.orderService = orderService;
        this.supplierOrderService = supplierOrderService;
        this.publishEndpoint = publishEndpoint;
        this.productService = productService;
        this.metrics = metrics;
        this.logger = logger;
    }

    public async Task<Result> Create(CreateOrderModel model, CancellationToken cancellationToken)
    {
        var strategy = this.context.Database.CreateExecutionStrategy();
        var qtys = model.Items.ToDictionary(x => x.ProductId, x => x.Qty);

        return await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await this.context.Database.BeginTransactionAsync(cancellationToken);

            var createOrderResult = await this.orderService.Create(model, cancellationToken);

            var products = await this.productService.GetProducts(model.Items.Select(x => x.ProductId), cancellationToken);

            foreach (var product in products)
            {
                await this.supplierOrderService.Create(product.SupplierId, product.Id, createOrderResult.Value, qtys[product.Id], cancellationToken);
            }

            await transaction.CommitAsync(cancellationToken);

            await this.publishEndpoint.Publish(
                new OrderCreatedBusEventModel(
                    Id: createOrderResult.Value,
                    CustomerId: model.CustomerId,
                    ProductIds: model.Items.Select(x => x.ProductId)), cancellationToken);

            this.metrics.OnNewOrder();

            return Result.Ok();
        });
    }
}