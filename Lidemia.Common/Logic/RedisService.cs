// Created on 25/11/2026 22:48 by Laserson

using Lidemia.Common.Logic.Abstract;
using Lidemia.Core.Extensions;
using Lidemia.Core.Models.Cache;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using System.Text.Json;

namespace Lidemia.Common.Logic;

[ServiceDescriptor<IRedisService>(ServiceLifetime.Singleton)]
public class RedisService : IRedisService
{
    private const int MaxViewedProducts = 100;

    private readonly IDistributedCache cache;
    private readonly IUtc utc;

    public RedisService(IDistributedCache cache, IUtc utc)
    {
        this.cache = cache;
        this.utc = utc;
    }

    public async Task AddCustomerViewedProduct(long customerId, long productId, CancellationToken cancellationToken)
    {
        var key = GetViewedProductsKey(customerId);
        var products = await this.cache.GetAsync(key, cancellationToken);

        if (products == null)
        {
            var newItems = new UserViewedProductsCachePayload(Items: new Dictionary<long, DateTimeOffset>
            {
                [productId] = this.utc.Now
            });

            products = JsonSerializer.SerializeToUtf8Bytes(newItems);
        }
        else
        {
            var existingProducts = JsonSerializer.Deserialize<UserViewedProductsCachePayload>(products).NotNull();
            existingProducts.Items[productId] = this.utc.Now;

            if (existingProducts.Items.Count >= MaxViewedProducts)
            {
                var filteredItems = existingProducts.Items
                    .OrderByDescending(x => x.Value)
                    .Take(MaxViewedProducts)
                    .ToDictionary(x => x.Key, x => x.Value);

                existingProducts = new UserViewedProductsCachePayload(Items: filteredItems);
            }

            products = JsonSerializer.SerializeToUtf8Bytes(existingProducts);
        }

        await this.cache.SetAsync(key, products, token: cancellationToken);
    }

    public async Task<UserViewedProductsCachePayload?> GetCustomerViewedProducts(long customerId, CancellationToken cancellationToken)
    {
        var key = GetViewedProductsKey(customerId);
        var entry = await this.cache.GetAsync(key, cancellationToken);

        return entry == null ? null : JsonSerializer.Deserialize<UserViewedProductsCachePayload>(entry);
    }

    private static string GetViewedProductsKey(long customerId) => $":viewed-products:{customerId}";
}