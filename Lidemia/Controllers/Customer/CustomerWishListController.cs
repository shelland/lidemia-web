// Created on 19/10/2026 22:10 by Laserson

using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Core.Attributes;
using Lidemia.Core.Models.Dto;
using Lidemia.Core.Models.Service;
using Microsoft.AspNetCore.Mvc;

namespace Lidemia.Controllers.Customer;

[CustomerAuthorize]
[Route("Customer/WishList")]
public class CustomerWishListController : BaseController
{
    private readonly IWishListService wishListService;

    public CustomerWishListController(IWishListService wishListService)
    {
        this.wishListService = wishListService;
    }

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] CustomerWishListFilterModel filter, CancellationToken cancellationToken)
    {
        var items = await this.wishListService.GetCustomerWishList(RequireEntityId, filter, cancellationToken);
        return View(items);
    }

    [HttpPost]
    public async Task<ResultInfo> Post([FromBody] AddToWishListRequestDto request, CancellationToken cancellationToken)
    {
        await this.wishListService.AddToWishList(RequireEntityId, request, cancellationToken);
        return ResultInfo.Ok;
    }
}