// Created on 01/09/2026 18:32 by Laserson

using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Lidemia.Controllers.Supplier;

public class SupplierSignInController : BaseController
{
    private readonly ISupplierSignInService signInService;

    public SupplierSignInController(ISupplierSignInService signInService)
    {
        this.signInService = signInService;
    }

    // GET
    public IActionResult Index()
    {
        return View();
    }
}