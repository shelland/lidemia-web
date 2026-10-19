// Created on 02/09/2026 20:10 by Laserson

using Lidemia.Core.Models.Misc;

namespace Lidemia.Common.BusinessLogic.Services.Data.Abstract;

public interface IShoppingCartService
{
    Task<IEnumerable<ShoppingCartItem>?> GetCustomerShoppingCart(long customerId, CancellationToken cancellationToken);

    Task AddProduct(long customerId, long productId, double qty, CancellationToken cancellationToken);
}