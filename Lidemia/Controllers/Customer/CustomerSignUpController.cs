// Created on 01/09/2026 18:32 by Laserson

using Microsoft.AspNetCore.Mvc;

namespace Lidemia.Controllers.Customer;

public class CustomerSignUpController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}