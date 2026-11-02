using Lidemia.Core.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Lidemia.Admin.Controllers;

[AdminAuthorize]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
