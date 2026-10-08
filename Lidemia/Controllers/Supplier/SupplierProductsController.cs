// Created on 03/09/2026 20:28 by Laserson

using FluentValidation;
using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Common.Validators;
using Lidemia.Core.Attributes;
using Lidemia.Core.Extensions;
using Lidemia.Core.Models.Dto;
using Lidemia.Core.Models.Misc;
using Lidemia.Core.Models.Service;
using Lidemia.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Lidemia.Controllers.Supplier;

[SupplierAuthorize]
[Route("Supplier/Products")]
public class SupplierProductsController : BaseController
{
    private readonly IProductService productService;
    private readonly IValidator<SaveProductRequestDto> validator;

    public SupplierProductsController(IProductService productService, IValidator<SaveProductRequestDto> validator)
    {
        this.productService = productService;
        this.validator = validator;
    }

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] PagingInfoModel pagingInfo, CancellationToken cancellationToken)
    {
        var products = await this.productService.GetSupplierProducts(RequireEntityId, pagingInfo, cancellationToken);
        return View(products);
    }

    [HttpGet("Create")]
    public async Task<IActionResult> Create()
    {
        return View(new CreateProductViewModel(null));
    }

    [HttpGet("Edit/{id:long}")]
    public async Task<IActionResult> Edit([FromRoute] long id, CancellationToken cancellationToken)
    {
        var product = await this.productService.GetById(id, cancellationToken);

        if (product == null || product.SupplierId != RequireEntityId)
        {
            return RedirectToAction("Index");
        }

        var viewModel = new CreateProductViewModel(product);
        return View("Create", viewModel);
    }

    [HttpPost]
    public async Task<ResultInfo> Save([FromBody] SaveProductRequestDto request, CancellationToken cancellationToken)
    {
        var validationResult = await this.validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return new ErrorResultInfo(validationResult.GetErrors());
        }

        var result = await productService.Save(RequireEntityId, request, cancellationToken);
        return ResultInfo.Ok;
    }

    [HttpPost("SetVisibility")]
    public async Task<ResultInfo> SetVisibility([FromBody] SetProductVisibilityRequestDto request, CancellationToken cancellationToken)
    {
        await this.productService.SetProductVisibility(RequireEntityId, request, cancellationToken);
        return ResultInfo.Ok;
    }
}