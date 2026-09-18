// Created on 01/09/2026 18:32 by Laserson

using FluentValidation;
using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Core.Enums;
using Lidemia.Core.Extensions;
using Lidemia.Core.Models.Dto;
using Lidemia.Core.Models.Service;
using Microsoft.AspNetCore.Mvc;

namespace Lidemia.Controllers.Supplier;

[Route("Supplier/SignIn")]
public class SupplierSignInController : BaseController
{
    private readonly ISupplierSignInService signInService;
    private readonly IValidator<SupplierSignInRequestDto> validator;

    public SupplierSignInController(ISupplierSignInService signInService, IValidator<SupplierSignInRequestDto> validator)
    {
        this.signInService = signInService;
        this.validator = validator;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<ResultInfo> Post(SupplierSignInRequestDto request, CancellationToken cancellationToken)
    {
        var validationResult = await this.validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return new ErrorResultInfo(validationResult.GetErrors());
        }

        var signInResult = await this.signInService.SignIn(request, cancellationToken);

        return signInResult.Status != LoginResultStatus.Success ? new ErrorResultInfo(["invalid_credentials"]) : ResultInfo.Ok;
    }
}