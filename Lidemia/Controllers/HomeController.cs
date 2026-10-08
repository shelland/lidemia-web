using Lidemia.Core.Models.Dto;
using Lidemia.Core.Models.Misc;
using Lidemia.Search.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Lidemia.Controllers;

public class HomeController : BaseController
{
    [HttpGet]
    public IActionResult Index([FromServices] ISearchService searchService, [FromServices] ILogger<HomeController> logger)
    {
        var products = new List<ProductMainInfoModel>
        {
            new(1, "Шоколад молочный", 250, 1, "Супер молочный шоколад", 1, ["шоколад", "молочный"], true),
            new(2, "Шоколад темный", 280, 1, "Супер темный шоколад", 1, ["шоколад", "темный"], true),
            new(3, "Шоколад белый", 200, 1, "Супер белый шоколад", 1, ["шоколад", "белый"], true)
        };

        // searchService.RebuildIndex(products);

        // searchService.UpdateInIndex(new ProductMainInfoModel(1, "Шоколад молочный", 250)).Wait();
        logger.LogInformation("Hello, {Guid}", Guid.NewGuid());

        var results = searchService.Search(new ProductsListFilterModel
        {
            Title = "белы"
        }).Result; 

        return View();
    }
}