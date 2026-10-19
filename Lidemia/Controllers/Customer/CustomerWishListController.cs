// Created on 19/10/2026 22:10 by Laserson

using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Core.Models.Dto;
using Lidemia.Core.Models.Service;
using Microsoft.AspNetCore.Mvc;

namespace Lidemia.Controllers.Customer;

[Microsoft.AspNetCore.Components.Route("Customer/WishList")]
public class CustomerWishListController : BaseController
{
    private readonly IWishListService wishListService;

    public CustomerWishListController(IWishListService wishListService)
    {
        this.wishListService = wishListService;
    }

    [HttpPost]
    public async Task<ResultInfo> Post([FromBody] AddToWishListRequestDto request, CancellationToken cancellationToken)
    {
        var result = await this.wishListService.AddToWishList(RequireEntityId, request, cancellationToken);
        return result ? ResultInfo.Ok : ErrorResultInfo.Error;
    }
}