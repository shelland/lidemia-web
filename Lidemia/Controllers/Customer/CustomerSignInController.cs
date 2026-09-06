// Created on 01/09/2026 18:32 by Laserson

using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Core.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Lidemia.Controllers.Customer;

public class CustomerSignInController : BaseController
{
    private readonly ICustomerSignInService signInService;

    public CustomerSignInController(ICustomerSignInService signInService)
    {
        this.signInService = signInService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task Post([FromBody] CustomerSignInRequestDto request, CancellationToken cancellationToken)
    {

    }
}