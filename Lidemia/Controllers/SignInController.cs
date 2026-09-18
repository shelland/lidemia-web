// Created on 01/09/2026 18:32 by Laserson

using Microsoft.AspNetCore.Mvc;

namespace Lidemia.Controllers;

public class SignInController : BaseController
{
    [HttpGet]
    public IActionResult Index() => View();
}