// Created on 19/10/2026 19:09 by Laserson

using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Core.Attributes;
using Lidemia.Core.Extensions;
using Lidemia.Core.Models.Dto;
using Lidemia.Core.Models.Service;
using Lidemia.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Lidemia.Controllers.Customer;

[CustomerAuthorize]
[Route("Customer/ShoppingCart")]
public class CustomerShoppingCartController : BaseController
{
    private readonly IShoppingCartService shoppingCartService;
    private readonly IProductService productService;

    public CustomerShoppingCartController(IShoppingCartService shoppingCartService, IProductService productService)
    {
        this.shoppingCartService = shoppingCartService;
        this.productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var items = await this.shoppingCartService.GetCustomerShoppingCart(RequireEntityId, cancellationToken);
        decimal totalSum = 0, totalDiscountSum = 0;

        foreach (var product in items ?? [])
        {
            if (product.Product.HasCurrentDiscount)
            {
                totalDiscountSum += (product.Product.DiscountPrice?.Value ?? 0) * product.Qty;
            }
            else
            {
                totalDiscountSum += product.Product.Price.NotNull().Value * product.Qty;
            }

            totalSum += product.Product.Price.NotNull().Value * product.Qty;
        }


        var viewModel = new ShoppingCartViewModel(Items: items, Total: totalSum, DiscountTotal: totalDiscountSum);

        return View(viewModel);
    }

    [HttpPost]
    public async Task<ResultInfo> Post([FromBody] AddToCartRequestModel request, CancellationToken cancellationToken)
    {
        await this.shoppingCartService.AddProduct(RequireEntityId, request.ProductId, request.Qty, cancellationToken);
        return ResultInfo.Ok;
    }

    [HttpGet("CartPreview")]
    public async Task<IActionResult> LoadCartPreview(CancellationToken cancellationToken)
    {
        var items = await this.shoppingCartService.GetCustomerShoppingCart(RequireEntityId, cancellationToken);
        return PartialView(items);
    }
}