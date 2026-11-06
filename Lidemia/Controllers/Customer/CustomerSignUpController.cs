// Created on 01/09/2026 18:32 by Laserson

using FluentValidation;
using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Core.Enums;
using Lidemia.Core.Extensions;
using Lidemia.Core.Models.Domain;
using Lidemia.Core.Models.Dto;
using Lidemia.Core.Models.Service;
using Lidemia.Web.BusinessLogic.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Lidemia.Controllers.Customer;

[Route("Customer/SignUp")]
public class CustomerSignUpController : BaseController
{
    private readonly ICustomerSignUpService signUpService;
    private readonly IWebAuthService webAuthService;
    private readonly ICustomerService customerService;
    private readonly IValidator<CustomerSignUpRequestDto> validator;

    public CustomerSignUpController(
        ICustomerSignUpService signUpService,
        IWebAuthService webAuthService,
        ICustomerService customerService,
        IValidator<CustomerSignUpRequestDto> validator)
    {
        this.signUpService = signUpService;
        this.webAuthService = webAuthService;
        this.customerService = customerService;
        this.validator = validator;
    }

    [HttpGet]
    public IActionResult Index() => View();

    [HttpPost]
    public async Task<ResultInfo> Post([FromBody] CustomerSignUpRequestDto request, CancellationToken cancellationToken)
    {
        var validationResult = await this.validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return new ErrorResultInfo(validationResult.GetErrors());
        }

        var signUpResult = await this.signUpService.SignUp(request, cancellationToken);
        var customer = await this.customerService.FindById(signUpResult.Value, cancellationToken);
        
        await this.webAuthService.AuthCustomer(new SignInResult<CustomerModel>(Status: LoginResultStatus.Success, Entity: customer), cancellationToken);
        return ResultInfo.Ok;
    }
}