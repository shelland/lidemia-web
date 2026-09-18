using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Core.Models.Dto;
using Lidemia.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Lidemia.Controllers;

public class ProductsController : BaseController
{
    private readonly IProductService productService;
    private readonly IPhotoService photoService;

    public ProductsController(IProductService productService, IPhotoService photoService)
    {
        this.productService = productService;
        this.photoService = photoService;
    }

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] ProductsListFilterModel? filter, CancellationToken cancellationToken)
    {
        filter ??= new ProductsListFilterModel();
        var products = await this.productService.GetPublicList(filter, cancellationToken);
        
        var viewModel = new ProductsListViewModel
        {
            CurrentPage = filter.Page,
            Filter = filter,
            Items = products.Items,
            TotalCount = products.TotalCount,
            TotalPages = products.TotalPages
        };

        return View(viewModel);
    }

    [HttpGet("Details/{id:long}")]
    public async Task<IActionResult> Details([FromRoute] long id, CancellationToken cancellationToken)
    {
        var product = await this.productService.GetById(id, cancellationToken);

        if (product == null)
        {
            return NotFoundView;
        }

        var photos = await this.photoService.GetProductPhotos(id, cancellationToken);

        var viewModel = new ProductDetailsViewModel(
            Product: product,
            Photos: photos
        );

        return View(viewModel);
    }
}