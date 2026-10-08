using Microsoft.AspNetCore.Mvc;

namespace Lidemia.Controllers;

public class TermsOfUseController : BaseController
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}