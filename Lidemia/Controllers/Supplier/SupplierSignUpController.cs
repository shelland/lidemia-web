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

namespace Lidemia.Controllers.Supplier;

[Route("Supplier/SignUp")]
public class SupplierSignUpController : BaseController
{
    private readonly ISupplierSignUpService signUpService;
    private readonly IWebAuthService webAuthService;
    private readonly ISupplierService supplierService;
    private readonly IValidator<SupplierSignUpRequestDto> validator;

    public SupplierSignUpController(ISupplierSignUpService signUpService, IWebAuthService webAuthService, ISupplierService supplierService,
        IValidator<SupplierSignUpRequestDto> validator)
    {
        this.signUpService = signUpService;
        this.webAuthService = webAuthService;
        this.supplierService = supplierService;
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

        var signUpResult = await this.signUpService.SignUp(request, cancellationToken);
        var supplier = await this.supplierService.FindById(signUpResult.Value, cancellationToken);
        await this.webAuthService.AuthSupplier(new SignInResult<SupplierModel>(LoginResultStatus.Success, supplier), cancellationToken);

        return ResultInfo.Ok;
    }
}