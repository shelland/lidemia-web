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

namespace Lidemia.Controllers.Supplier;

[Route("Supplier/SignIn")]
public class SupplierSignInController : BaseController
{
    private readonly ISupplierSignInService signInService;
    private readonly IValidator<SupplierSignInRequestDto> validator;
    private readonly IWebAuthService webAuthService;

    public SupplierSignInController(ISupplierSignInService signInService, IValidator<SupplierSignInRequestDto> validator, IWebAuthService webAuthService)
    {
        this.signInService = signInService;
        this.validator = validator;
        this.webAuthService = webAuthService;
    }

    [HttpGet]
    public IActionResult Index() => View();

    [HttpPost]
    public async Task<ResultInfo> Post([FromBody] SupplierSignInRequestDto request, CancellationToken cancellationToken)
    {
        var validationResult = await this.validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return new ErrorResultInfo(validationResult.GetErrors());
        }

        var signInResult = await this.signInService.SignIn(request, cancellationToken);

        if (signInResult.Status == LoginResultStatus.Success)
        {
            await this.webAuthService.AuthSupplier(signInResult!, cancellationToken);
        }
        
        return signInResult.Status != LoginResultStatus.Success ? Errors.InvalidCredentials : ResultInfo.Ok;
    }
}