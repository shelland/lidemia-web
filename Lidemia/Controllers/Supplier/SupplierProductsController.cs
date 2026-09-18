// Created on 03/09/2026 20:28 by Laserson

using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Core.Attributes;
using Lidemia.Core.Models.Misc;
using Microsoft.AspNetCore.Mvc;

namespace Lidemia.Controllers.Supplier;

[SupplierAuthorize]
[Route("Supplier/Products")]
public class SupplierProductsController : BaseController
{
    private readonly IProductService productService;

    public SupplierProductsController(IProductService productService)
    {
        this.productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] PagingInfoModel pagingInfo, CancellationToken cancellationToken)
    {
        var products = await this.productService.GetSupplierProducts(RequireEntityId, pagingInfo, cancellationToken);
        return View(products);
    }
}