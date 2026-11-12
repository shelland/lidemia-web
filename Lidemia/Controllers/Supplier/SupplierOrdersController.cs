// Created on 10/11/2026 12:22 by Laserson

using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Core.Attributes;
using Lidemia.Core.Models.Dto;
using Lidemia.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Lidemia.Controllers.Supplier;

[Route("Supplier/Orders")]
[SupplierAuthorize]
public class SupplierOrdersController : BaseController
{
    private readonly ISupplierOrderService supplierOrderService;
    private readonly IProductService productService;

    public SupplierOrdersController(ISupplierOrderService supplierOrderService, IProductService productService)
    {
        this.supplierOrderService = supplierOrderService;
        this.productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] SupplierOrdersFilterModel filter, CancellationToken cancellationToken)
    {
        var orders = await this.supplierOrderService.GetSupplierOrders(RequireEntityId, filter, cancellationToken);
        return View(orders);
    }

    [HttpGet("Details/{id:long}")]
    public async Task<IActionResult> Details([FromRoute] long id, CancellationToken cancellationToken)
    {
        var order = await this.supplierOrderService.GetSupplierOrder(id, RequireEntityId, cancellationToken);

        if (order == null)
        {
            return NotFoundView;
        }

        var products = await this.productService.GetProducts(order.Items.Items.Select(x => x.ProductId), cancellationToken);
        var viewModel = new SupplierOrderDetailsViewModel(
            Order: order,
            Products: products
        );

        return View(viewModel);
    }
}