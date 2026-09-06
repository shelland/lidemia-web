// Created on 03/09/2026 20:28 by Laserson

using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Core.Attributes;
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

    // GET
    public IActionResult Index()
    {
        return View();
    }
}