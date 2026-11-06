// Created on 03/09/2026 18:56 by Laserson

using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Repository.Abstract.Base;

namespace Lidemia.DataAccess.Repository.Abstract;

public interface IShoppingCartRepository : IRepository<ShoppingCartEntity, long>
{
    Task<ShoppingCartEntity?> GetCustomerShoppingCart(long customerId, CancellationToken cancellationToken);

    Task AddProduct(long customerId, long productId, int qty, CancellationToken cancellationToken);

    Task<Dictionary<long, int>?> GetListItems(long customerId, CancellationToken cancellationToken);
}