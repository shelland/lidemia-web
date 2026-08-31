using Microsoft.AspNetCore.Mvc;

namespace Lidemia.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
