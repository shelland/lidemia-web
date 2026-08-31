using Microsoft.AspNetCore.Mvc;

namespace Lidemia.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
