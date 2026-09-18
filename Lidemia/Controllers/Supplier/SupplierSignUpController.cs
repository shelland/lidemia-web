// Created on 01/09/2026 18:32 by Laserson

using FluentValidation;
using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Core.Extensions;
using Lidemia.Core.Models.Dto;
using Lidemia.Core.Models.Service;
using Microsoft.AspNetCore.Mvc;

namespace Lidemia.Controllers.Supplier;

[Route("Supplier/SignUp")]
public class SupplierSignUpController : BaseController
{
    private readonly ISupplierSignUpService signUpService;
    private readonly IValidator<SupplierSignUpRequestDto> validator;

    public SupplierSignUpController(ISupplierSignUpService signUpService, IValidator<SupplierSignUpRequestDto> validator)
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
    public async Task<ResultInfo> Post([FromBody] SupplierSignUpRequestDto request, CancellationToken cancellationToken)
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