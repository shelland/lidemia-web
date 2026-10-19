// Created on 19/10/2026 19:09 by Laserson

using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Core.Attributes;
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

    public CustomerShoppingCartController(IShoppingCartService shoppingCartService)
    {
        this.shoppingCartService = shoppingCartService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var items = await this.shoppingCartService.GetCustomerShoppingCart(RequireEntityId, cancellationToken);
        var viewModel = new ShoppingCartViewModel(Items: items);

        return View(viewModel);
    }

    [HttpPost]
    public async Task<ResultInfo> Post([FromBody] AddToCartRequestModel request, CancellationToken cancellationToken)
    {
        await this.shoppingCartService.AddProduct(RequireEntityId, request.ProductId, request.Qty, cancellationToken);
        return ResultInfo.Ok;
    }
}