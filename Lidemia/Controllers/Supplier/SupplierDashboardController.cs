// Created on 01/09/2026 14:28 by Laserson

using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Core.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Lidemia.Controllers.Supplier;

[SupplierAuthorize]
[Route("Supplier/Dashboard")]
public class SupplierDashboardController : BaseController
{
    private readonly ISupplierOrderService supplierOrderService;

    public SupplierDashboardController(ISupplierOrderService supplierOrderService)
    {
        this.supplierOrderService = supplierOrderService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}