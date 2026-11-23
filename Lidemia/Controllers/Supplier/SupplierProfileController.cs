// Created on 22/11/2026 14:35 by Laserson

using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Core.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Lidemia.Controllers.Supplier;

[Route("Supplier/Profile")]
[SupplierAuthorize]
public class SupplierProfileController : BaseController
{
    private readonly ISupplierService supplierService;

    public SupplierProfileController(ISupplierService supplierService)
    {
        this.supplierService = supplierService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View();
    }

    [HttpGet("Edit")]
    public async Task<IActionResult> Edit(CancellationToken cancellationToken)
    {
        var supplier = await this.supplierService.FindById(RequireEntityId, cancellationToken);

        if (supplier == null)
        {
            return NotFoundView;
        }

        return View(supplier);
    }
}