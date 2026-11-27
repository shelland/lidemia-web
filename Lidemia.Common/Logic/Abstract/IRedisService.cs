// Created on 25/11/2026 22:48 by Laserson

using Lidemia.Core.Models.Cache;

namespace Lidemia.Common.Logic.Abstract;

public interface IRedisService
{
    Task AddCustomerViewedProduct(long customerId, long productId, CancellationToken cancellationToken);

    Task<UserViewedProductsCachePayload?> GetCustomerViewedProducts(long customerId, CancellationToken cancellationToken);
}