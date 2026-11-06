using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Common.BusinessLogic.Services.Providers.Abstract;
using Lidemia.Core.Enums;
using Lidemia.Core.Models.Domain;
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
    private readonly IProductsProvider productsProvider;

    public ProductsController(
        IProductService productService,
        IProductPhotoService productPhotoService,
        IShoppingCartService shoppingCartService,
        IProductCategoryService productCategoryService,
        IProductReviewService productReviewService,
        IOrderService orderService,
        IProductsProvider productsProvider)
    {
        this.productService = productService;
        this.productPhotoService = productPhotoService;
        this.shoppingCartService = shoppingCartService;
        this.productCategoryService = productCategoryService;
        this.productReviewService = productReviewService;
        this.orderService = orderService;
        this.productsProvider = productsProvider;
    }

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] ProductsListFilterModel? filter, CancellationToken cancellationToken)
    {
        filter ??= new ProductsListFilterModel();
        var viewModel = await GetListViewModel(filter, cancellationToken);

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

        var viewModel = await GetDetailsViewModel(product, cancellationToken);
        return View(viewModel);
    }

    private async Task<ProductDetailsViewModel> GetDetailsViewModel(ProductModel product, CancellationToken cancellationToken)
    {
        var photos = await this.productPhotoService.GetProductPhotos(product.Id, cancellationToken);
        var relatedProducts = await this.productService.GetRelatedProducts(product.Category!.Id, product.Id, cancellationToken: cancellationToken);
        var reviews = await this.productReviewService.GetRecentReviews(product.Id, cancellationToken: cancellationToken);

        var isAbleToPurchase = EntityId != null && EntityType == EntityType.Customer;
        int? itemsInCart = null;

        if (isAbleToPurchase)
        {
            var userCart = await this.shoppingCartService.GetCustomerShoppingCart(RequireEntityId, cancellationToken);
            itemsInCart = userCart?.FirstOrDefault(x => x.Product.Id == product.Id)?.Qty;
        }

        var viewModel = new ProductDetailsViewModel(
            IsAbleToPurchase: isAbleToPurchase,
            ItemsInCart: itemsInCart,
            Product: product,
            CurrentUserOrder: EntityId != null ? await this.orderService.GetUserProductLastOrder(RequireEntityId, product.Id, cancellationToken) : null,
            CurrentUserReview: EntityId != null ? await this.productReviewService.GetUserReview(RequireEntityId, product.Id, cancellationToken) : null,
            Photos: photos,
            RelatedProducts: relatedProducts,
            RecentReviews: reviews
        );

        return viewModel;
    }

    private async Task<ProductsListViewModel> GetListViewModel(ProductsListFilterModel filter, CancellationToken cancellationToken)
    {
        var products = await this.productsProvider.GetPublicList(filter, cancellationToken);
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