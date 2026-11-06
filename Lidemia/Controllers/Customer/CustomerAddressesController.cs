// Created on 02/10/2026 18:55 by Laserson

using Lidemia.Core.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Lidemia.Controllers.Customer;

[CustomerAuthorize]
[Route("Customer/Addresses")]
public class CustomerAddressesController : BaseController
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View();
    }
}