using Microsoft.AspNetCore.Mvc;

namespace Lidemia.Controllers;

public class HomeController : BaseController
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}
