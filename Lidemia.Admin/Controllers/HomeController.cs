using Microsoft.AspNetCore.Mvc;

namespace Lidemia.Admin.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
