// Created on 01/09/2026 18:32 by Laserson

using FluentValidation;
using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Core.Extensions;
using Lidemia.Core.Models.Dto;
using Lidemia.Core.Models.Service;
using Microsoft.AspNetCore.Mvc;

namespace Lidemia.Controllers.Customer;

[Route("Customer/SignUp")]
public class CustomerSignUpController : BaseController
{
    private readonly ICustomerSignUpService signUpService;
    private readonly IValidator<CustomerSignUpRequestDto> validator;

    public CustomerSignUpController(ICustomerSignUpService signUpService, IValidator<CustomerSignUpRequestDto> validator)
    {
        this.signUpService = signUpService;
        this.validator = validator;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<ResultInfo> Post([FromBody] CustomerSignUpRequestDto request, CancellationToken cancellationToken)
    {
        var validationResult = await this.validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return new ErrorResultInfo(validationResult.GetErrors());
        }

        await this.signUpService.SignUp(request, cancellationToken);
        return ResultInfo.Ok;
    }
}