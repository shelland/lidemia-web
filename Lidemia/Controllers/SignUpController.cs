// Created on 01/09/2026 18:33 by Laserson

using Microsoft.AspNetCore.Mvc;

namespace Lidemia.Controllers;

public class SignUpController : BaseController
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}