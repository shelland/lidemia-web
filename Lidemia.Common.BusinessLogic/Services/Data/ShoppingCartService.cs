// Created on 19/10/2026 19:08 by Laserson

using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Core.Models.Misc;
using Lidemia.DataAccess.Repository.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.Common.BusinessLogic.Services.Data;

[ServiceDescriptor<IShoppingCartService>(ServiceLifetime.Scoped)]
public class ShoppingCartService : IShoppingCartService
{
    private readonly IShoppingCartRepository shoppingCartRepository;
    private readonly IProductService productService;

    public ShoppingCartService(IShoppingCartRepository shoppingCartRepository, IProductService productService)
    {
        this.shoppingCartRepository = shoppingCartRepository;
        this.productService = productService;
    }

    public async Task<IEnumerable<ShoppingCartItem>?> GetCustomerShoppingCart(long customerId, CancellationToken cancellationToken)
    {
        var cart = await this.shoppingCartRepository.GetCustomerShoppingCart(customerId, cancellationToken);

        if (cart == null)
        {
            return null;
        }

        var products = (await this.productService.GetProducts(cart.Items.Select(x => x.ProductId), cancellationToken))
            .ToDictionary(x => x.Id, x => x);

        return cart.Items.Select(x => new ShoppingCartItem(Product: products[x.ProductId], Qty: x.Qty));
    }

    public Task AddProduct(long customerId, long productId, double qty, CancellationToken cancellationToken)
    {
        return this.shoppingCartRepository.AddProduct(customerId, productId, qty, cancellationToken);
    }
}