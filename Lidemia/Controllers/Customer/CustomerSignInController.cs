// Created on 01/09/2026 18:32 by Laserson

using FluentValidation;
using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Core;
using Lidemia.Core.Enums;
using Lidemia.Core.Extensions;
using Lidemia.Core.Models.Dto;
using Lidemia.Core.Models.Service;
using Lidemia.Web.BusinessLogic.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Lidemia.Controllers.Customer;

[Route("Customer/SignIn")]
public class CustomerSignInController : BaseController
{
    private readonly ICustomerSignInService signInService;
    private readonly IValidator<CustomerSignInRequestDto> validator;
    private readonly IWebAuthService webAuthService;

    public CustomerSignInController(ICustomerSignInService signInService, IValidator<CustomerSignInRequestDto> validator, IWebAuthService webAuthService)
    {
        this.signInService = signInService;
        this.validator = validator;
        this.webAuthService = webAuthService;
    }

    [HttpGet]
    public IActionResult Index() => View();

    [HttpPost]
    public async Task<ResultInfo> Post([FromBody] CustomerSignInRequestDto request, CancellationToken cancellationToken)
    {
        var validationResult = await this.validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return new ErrorResultInfo(validationResult.GetErrors());
        }

        var signInResult = await this.signInService.SignIn(request, cancellationToken);

        if (signInResult.Status != LoginResultStatus.Success)
        {
            return Errors.InvalidCredentials;
        }

        await this.webAuthService.AuthCustomer(signInResult!, cancellationToken);
        return ResultInfo.Ok;

    }
}