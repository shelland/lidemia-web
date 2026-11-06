// Created on 03/11/2026 17:54 by Laserson

using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Core.Attributes;
using Lidemia.Core.Models.Domain;
using Lidemia.Core.Models.Dto;
using Lidemia.Core.Models.Service;
using Lidemia.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Lidemia.Controllers.Customer;

[Route("Customer/Checkout")]
[CustomerAuthorize]
public class CustomerCheckoutController : BaseController
{
    private readonly IShoppingCartService shoppingCartService;
    private readonly IProductService productService;
    private readonly ISupplierService supplierService;
    private readonly ICreateOrderService createOrderService;

    public CustomerCheckoutController(
        IShoppingCartService shoppingCartService,
        IProductService productService,
        ISupplierService supplierService, 
        ICreateOrderService createOrderService)
    {
        this.shoppingCartService = shoppingCartService;
        this.productService = productService;
        this.supplierService = supplierService;
        this.createOrderService = createOrderService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var cartItems = await this.shoppingCartService.GetListItems(RequireEntityId, cancellationToken);
        var productItems = new Dictionary<(long, string), IEnumerable<(ProductModel Product, int Qty)>>();

        if (cartItems != null)
        {
            var products = (await this.productService.GetProducts(cartItems.Select(x => x.Key), cancellationToken)).ToList();
            var groupedProducts = products.GroupBy(x => x.SupplierId);
            var suppliers = (await this.supplierService.GetSuppliers(products.Select(x => x.SupplierId), cancellationToken)).ToDictionary(x => x.Id);

            foreach (var group in groupedProducts)
            {
                productItems[(group.Key, suppliers[group.Key].Name)] = group.Select(x => (x, cartItems[x.Id]));
            }
        }

        var sums = productItems.Values
            .SelectMany(x => x)
            .Select(x => x.Product)
            .Select(x => x.HasCurrentDiscount ? x.DiscountPrice : x.Price)
            .Sum();

        var viewModel = new CheckoutViewModel(CartTotal: sums ?? 0, Items: productItems);
        return View(viewModel);
    }

    [HttpPost]
    public async Task<ResultInfo> Post([FromBody] CreateOrderRequestDto request, CancellationToken cancellationToken)
    {
        //var order = await this.orderService.Create(
        //    new CreateOrderModel(
        //        CustomerId: RequireEntityId,
        //        Comment: request.Comment,
        //        PromoCodeId: request.PromoCodeId,
        //        Items: request.Items.Select(x => new CreateOrderItemModel(ProductId: x.ProductId, Qty: x.Qty))), cancellationToken);


        // await this.createOrderService.Create(new CreateOrderModel(CustomerId:RequireEntityId,Comment:request.Comment,Pro))

        return ResultInfo.Ok;
    }

    [HttpPost("VerifyCart")]
    public async Task<ResultInfo> VerifyCart([FromBody] VerifyCartRequestDto request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}