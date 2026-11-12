// Created on 04/11/2026 14:21 by Laserson

using FluentResults;
using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Common.Metrics;
using Lidemia.Core.Errors;
using Lidemia.Core.Extensions;
using Lidemia.Core.Models.Bus;
using Lidemia.Core.Models.Service;
using Lidemia.DataAccess.Context;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;
using Scrutor;
using System.Data;

namespace Lidemia.Common.BusinessLogic.Services.Data;

[ServiceDescriptor<ICreateOrderService>(ServiceLifetime.Scoped)]
public class CreateOrderService : ICreateOrderService
{
    private const int MaxAttempts = 3;
    private const int BaseRetryDelayMs = 50;
    private const int MaxRetryDelayMs = 500;

    private readonly LidemiaDbContext context;
    private readonly IOrderService orderService;
    private readonly ISupplierOrderService supplierOrderService;
    private readonly IPublishEndpoint publishEndpoint;
    private readonly IProductService productService;
    private readonly IShoppingCartService shoppingCartService;
    private readonly OrderMetrics metrics;
    private readonly ILogger<CreateOrderService> logger;

    public CreateOrderService(
        LidemiaDbContext context,
        IOrderService orderService,
        ISupplierOrderService supplierOrderService,
        IPublishEndpoint publishEndpoint,
        IProductService productService,
        IShoppingCartService shoppingCartService,
        OrderMetrics metrics,
        ILogger<CreateOrderService> logger)
    {
        this.context = context;
        this.orderService = orderService;
        this.supplierOrderService = supplierOrderService;
        this.publishEndpoint = publishEndpoint;
        this.productService = productService;
        this.shoppingCartService = shoppingCartService;
        this.metrics = metrics;
        this.logger = logger;
    }

    public async Task<Result<CreateOrderResult>> Create(long customerId, CreateOrderServiceModel model, CancellationToken cancellationToken)
    {
        for (int attempt = 1;; attempt++)
        {
            try
            {
                return await this.CreateInTransaction(customerId, model, cancellationToken);
            }
            catch (Exception exception) when (attempt < MaxAttempts && IsRetryableConflict(exception))
            {
                this.logger.LogWarning(
                    exception,
                    "Order creation failed because of a transaction conflict, retrying (attempt {Attempt} of {MaxAttempts})",
                    attempt,
                    MaxAttempts);

                await Task.Delay(GetRetryDelay(attempt), cancellationToken);
            }
        }
    }

    private async Task<Result<CreateOrderResult>> CreateInTransaction(long customerId, CreateOrderServiceModel model, CancellationToken cancellationToken)
    {
        var strategy = this.context.Database.CreateExecutionStrategy();

        return await strategy.ExecuteAsync(async () =>
        {
            this.context.ChangeTracker.Clear();

            await using var transaction = await this.context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);

            var shoppingCartItems = (await this.shoppingCartService.GetCustomerShoppingCart(customerId, cancellationToken)).NotNull().ToList();
            var quantities = shoppingCartItems.ToDictionary(x => x.Product.Id, x => x.Qty);

            var reduceResult = await this.productService.ReduceQty(quantities, cancellationToken);

            if (reduceResult.Count > 0)
            {
                this.logger.LogError("Failed to reduce products {$Items}", reduceResult);
                return Result.Fail(new CreateOrderResultError(reduceResult));
            }

            var groupedAddresses = model.Items.ToDictionary(x => x.SupplierId, x => (x.SupplierAddressId, x.PromoCode));

            var createOrderResult = await this.orderService.Create(customerId, new CreateOrderModel(
                Comment: model.Comment,
                CustomerAddressId: model.CustomerAddressId,
                Items: shoppingCartItems.Select(x => new CreateOrderItemModel(
                    ProductId: x.Product.Id,
                    PromoCode: null,
                    SupplierAddressId: groupedAddresses.TryGetValue(x.Product.SupplierId, out var addressItem)
                        ? addressItem.SupplierAddressId
                        : null,
                    Qty: quantities[x.Product.Id]
                ))), cancellationToken);

            var products = await this.productService.GetProducts(quantities.Keys, cancellationToken);
            var groupedProducts = products.GroupBy(x => x.SupplierId, x => x);

            foreach (var grouped in groupedProducts)
            {
                await this.supplierOrderService.Create(grouped.Key,
                    groupedAddresses.TryGetValue(grouped.Key, out var addressItem) ? addressItem.SupplierAddressId : null, model.CustomerAddressId,
                    grouped.Select(x => (
                        ProductId: x.Id, 
                        Quantity: quantities[x.Id], 
                        Price: x.Price,
                        DiscountPrice: x.HasCurrentDiscount ? x.DiscountPrice : null)), createOrderResult.Value,
                    cancellationToken);
            }

            await transaction.CommitAsync(cancellationToken);

            await this.publishEndpoint.Publish(
                new OrderCreatedBusEventModel(
                    Id: createOrderResult.Value,
                    CustomerId: customerId,
                    ProductIds: quantities.Keys), cancellationToken);

            this.metrics.OnNewOrder();

            return Result.Ok(new CreateOrderResult(createOrderResult.Value));
        });
    }

    private static bool IsRetryableConflict(Exception exception)
    {
        for (Exception? current = exception; current != null; current = current.InnerException)
        {
            if (current is PostgresException { SqlState: PostgresErrorCodes.SerializationFailure or PostgresErrorCodes.DeadlockDetected })
            {
                return true;
            }
        }

        return false;
    }

    private static TimeSpan GetRetryDelay(int attempt)
    {
        int jitter = Random.Shared.Next(0, BaseRetryDelayMs);
        int delay = Math.Min((BaseRetryDelayMs * (1 << (attempt - 1))) + jitter, MaxRetryDelayMs);

        return TimeSpan.FromMilliseconds(delay);
    }
}

public record CreateOrderResult
(
    long OrderId
);