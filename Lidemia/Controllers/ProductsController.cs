using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Core.Models.Dto;
using Lidemia.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Lidemia.Controllers;

[Route("Products")]
public class ProductsController : BaseController
{
    private readonly IProductService productService;
    private readonly IProductPhotoService productPhotoService;
    private readonly IShoppingCartService shoppingCartService;
    private readonly IProductCategoryService productCategoryService;
    private readonly IProductReviewService productReviewService;
    private readonly IOrderService orderService;

    public ProductsController(
        IProductService productService,
        IProductPhotoService productPhotoService,
        IShoppingCartService shoppingCartService,
        IProductCategoryService productCategoryService,
        IProductReviewService productReviewService,
        IOrderService orderService)
    {
        this.productService = productService;
        this.productPhotoService = productPhotoService;
        this.shoppingCartService = shoppingCartService;
        this.productCategoryService = productCategoryService;
        this.productReviewService = productReviewService;
        this.orderService = orderService;
    }

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] ProductsListFilterModel? filter, CancellationToken cancellationToken)
    {
        filter ??= new ProductsListFilterModel();
        var viewModel = await PrepareViewModel(filter, cancellationToken);

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

        var photos = await this.productPhotoService.GetProductPhotos(id, cancellationToken);
        var relatedProducts = await this.productService.GetRelatedProducts(product.Category!.Id, cancellationToken: cancellationToken);
        var reviews = await this.productReviewService.GetRecentReviews(id, cancellationToken: cancellationToken);

        var viewModel = new ProductDetailsViewModel(
            Product: product,
            CurrentUserOrder: EntityId != null ? await this.orderService.GetUserProductLastOrder(RequireEntityId, id, cancellationToken) : null,
            CurrentUserReview: EntityId != null ? await this.productReviewService.GetUserReview(RequireEntityId, id, cancellationToken) : null,
            Photos: photos,
            RelatedProducts: relatedProducts,
            RecentReviews: reviews
        );

        return View(viewModel);
    }

    private async Task<ProductsListViewModel> PrepareViewModel(ProductsListFilterModel filter, CancellationToken cancellationToken)
    {
        var products = await this.productService.GetPublicList(filter, cancellationToken);
        var categories = (await this.productCategoryService.GetCategories(cancellationToken)).ToList();

        var newCategories = categories.ToLookup(x => x.Parent?.Id);

        foreach (var c in categories)
        {
            c.Children = newCategories[c.Id].ToList();
        }

        var roots = newCategories[null].ToList(); // ParentId == null

        var viewModel = new ProductsListViewModel
        {
            CurrentPage = filter.Page,
            Filter = filter,
            Items = products.Items,
            TotalCount = products.TotalCount,
            TotalPages = products.TotalPages,
            Categories = roots
        };

        if (EntityId != null)
        {
            var cartItems = await this.shoppingCartService.GetListItems(EntityId.Value, cancellationToken);
            viewModel.CartItems = cartItems;
        }

        return viewModel;
    }
}