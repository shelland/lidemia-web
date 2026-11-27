// Created on 20/11/2026 17:04 by Laserson

using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Lidemia.Controllers;

[Route("[controller]")]
public class SuppliersController : BaseController
{
    private readonly ISupplierService supplierService;

    public SuppliersController(ISupplierService supplierService)
    {
        this.supplierService = supplierService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("Details/{id:long}")]
    public async Task<IActionResult> Details([FromRoute] long id, CancellationToken cancellationToken)
    {
        var supplier = await this.supplierService.FindById(id, cancellationToken);

        if (supplier == null)
        {
            return NotFoundView;
        }

        var viewModel = new SupplierDetailsViewModel(
            TotalOrders: supplier.Stats?.TotalOrders ?? 0,
            TotalProducts: supplier.Stats?.TotalProducts ?? 0,
            AverageRate: supplier.Stats?.AverageRating ?? 0,
            Flags: supplier.Metadata.Flags,
            Supplier: supplier
        );

        return View(viewModel);
    }
}