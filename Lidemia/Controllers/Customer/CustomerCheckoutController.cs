// Created on 03/11/2026 17:54 by Laserson

using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Common.Extensions;
using Lidemia.Core.Attributes;
using Lidemia.Core.Errors;
using Lidemia.Core.Models.Domain;
using Lidemia.Core.Models.Dto;
using Lidemia.Core.Models.Misc;
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
    private readonly ICustomerAddressService customerAddressService;
    private readonly ISupplierAddressService supplierAddressService;
    private readonly IOrderService orderService;

    public CustomerCheckoutController(
        IShoppingCartService shoppingCartService,
        IProductService productService,
        ISupplierService supplierService,
        ICreateOrderService createOrderService,
        ICustomerAddressService customerAddressService,
        ISupplierAddressService supplierAddressService,
        IOrderService orderService)
    {
        this.shoppingCartService = shoppingCartService;
        this.productService = productService;
        this.supplierService = supplierService;
        this.createOrderService = createOrderService;
        this.customerAddressService = customerAddressService;
        this.supplierAddressService = supplierAddressService;
        this.orderService = orderService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var viewModel = await GetViewModel(cancellationToken);

        if (viewModel == null)
        {
            return RedirectToAction("Index", "CustomerShoppingCart");
        }

        return View(viewModel);
    }

    [HttpPost]
    public async Task<ResultInfo> Post([FromBody] CreateOrderRequestDto request, CancellationToken cancellationToken)
    {
        var result = await this.createOrderService.Create(RequireEntityId, new CreateOrderServiceModel(
                Comment: request.Comment,
                CustomerAddressId: request.CustomerAddressId,
                Items: request.Items.Select(x => new CreateOrderServiceModelItem(
                    PromoCode: x.PromoCode,
                    SupplierAddressId: x.SupplierAddressId,
                    SupplierId: x.SupplierId))),
            cancellationToken);

        if (result.IsFailed)
        {
            var error = result.GetDefaultError<CreateOrderResultError>();
            return new ErrorResultInfo<CreateOrderResultError>(error);
        }

        return new ResultInfo<long>(result.Value.OrderId);
    }

    [HttpPost("VerifyCart")]
    public async Task<ResultInfo<VerifyCartResponseDto>> VerifyCart([FromBody] VerifyCartRequestDto request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    [HttpGet("Finish/{id:long?}")]
    public async Task<IActionResult> Finish([FromRoute] long id, CancellationToken cancellationToken)
    {
        var order = await this.orderService.GetCustomerOrder(id, RequireEntityId, cancellationToken);
        return order == null ? NotFoundView : View(order);
    }

    private async Task<CheckoutViewModel?> GetViewModel(CancellationToken cancellationToken)
    {
        var cartItems = await this.shoppingCartService.GetListItems(RequireEntityId, cancellationToken);
        var productItems =
            new Dictionary<(long SupplierId, string SupplierName, IEnumerable<SupplierAddressModel> SupplierAddresses),
                IEnumerable<(ProductModel Product, int Qty)>>();

        if (cartItems == null || cartItems.Count == 0)
        {
            return null;
        }

        var products = (await this.productService.GetProducts(cartItems.Select(x => x.Key), cancellationToken)).ToList();
        var groupedProducts = products.GroupBy(x => x.SupplierId);
        var suppliers = (await this.supplierService.GetSuppliers(products.Select(x => x.SupplierId), cancellationToken)).ToDictionary(x => x.Id);
        var supplierAddresses = await this.supplierAddressService.GetAddresses(suppliers.Keys, cancellationToken);
        var groupedAddresses = supplierAddresses.GroupBy(x => x.SupplierId).ToDictionary(x => x.Key, x => x);

        foreach (var group in groupedProducts)
        {
            productItems
                    [(group.Key, suppliers[group.Key].Name, groupedAddresses.TryGetValue(group.Key, out var tempAddresses) ? tempAddresses : [])] =
                group.Select(x => (x, cartItems[x.Id]));
        }

        decimal totalSum = 0, totalDiscountSum = 0;

        foreach (var product in productItems.Values.SelectMany(x => x))
        {
            if (product.Product.HasCurrentDiscount)
            {
                totalDiscountSum += (product.Product.DiscountPrice ?? 0) * product.Qty;
            }
            else
            {
                totalDiscountSum += product.Product.Price * product.Qty;
            }

            totalSum += product.Product.Price * product.Qty;
        }

        var addresses = await this.customerAddressService.GetCustomerAddresses(RequireEntityId, new PagingInfoModel(), cancellationToken);

        var viewModel = new CheckoutViewModel(
            Total: totalSum,
            DiscountTotal: totalDiscountSum,
            Items: productItems,
            CustomerAddresses: addresses.Items
        );

        return viewModel;
    }
}