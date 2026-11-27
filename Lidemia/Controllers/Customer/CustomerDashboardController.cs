// Created on 01/09/2026 14:28 by Laserson

using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Common.Logic.Abstract;
using Lidemia.Core.Attributes;
using Lidemia.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Lidemia.Controllers.Customer;

[CustomerAuthorize]
[Route("Customer/Dashboard")]
public class CustomerDashboardController : BaseController
{
    private readonly IRedisService redisService;
    private readonly IProductService productService;

    public CustomerDashboardController(IRedisService redisService, IProductService productService)
    {
        this.redisService = redisService;
        this.productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var viewedProductsCache = await this.redisService.GetCustomerViewedProducts(RequireEntityId, cancellationToken);
        var viewedProducts = viewedProductsCache != null ? await this.productService.GetProducts(viewedProductsCache.Items.Keys, cancellationToken) : [];

        var viewModel = new CustomerDashboardViewModel(ViewedProducts: viewedProducts);
        return View(viewModel);
    }
}