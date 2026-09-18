// Created on 07/09/2026 20:12 by Laserson

using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Core.Attributes;
using Lidemia.Core.Models.Misc;
using Microsoft.AspNetCore.Mvc;

namespace Lidemia.Controllers.Customer;

[CustomerAuthorize]
[Route("Customer/Orders")]
public class CustomerOrdersController : BaseController
{
    private readonly IOrderService orderService;

    public CustomerOrdersController(IOrderService orderService)
    {
        this.orderService = orderService;
    }

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] PagingInfoModel pagingInfo, CancellationToken cancellationToken)
    {
        var orders = await this.orderService.GetCustomerOrders(pagingInfo, RequireEntityId, cancellationToken);
        return View(orders);
    }
}