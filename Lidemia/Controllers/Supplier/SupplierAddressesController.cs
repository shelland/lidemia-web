// Created on 11/10/2026 15:30 by Laserson

using FluentValidation;
using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Core.Attributes;
using Lidemia.Core.Extensions;
using Lidemia.Core.Models.Domain;
using Lidemia.Core.Models.Service;
using Lidemia.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Lidemia.Controllers.Supplier;

[SupplierAuthorize]
[Route("Supplier/Addresses")]
public class SupplierAddressesController : BaseController
{
    private readonly ISupplierAddressService supplierAddressService;
    private readonly ICountryService countryService;
    private readonly IRegionService regionService;
    private readonly ICityService cityService;
    private readonly IValidator<SaveSupplierAddressRequestDto> validator;

    public SupplierAddressesController(
        ISupplierAddressService supplierAddressService,
        ICountryService countryService,
        IRegionService regionService,
        ICityService cityService,
        IValidator<SaveSupplierAddressRequestDto> validator)
    {
        this.supplierAddressService = supplierAddressService;
        this.countryService = countryService;
        this.regionService = regionService;
        this.cityService = cityService;
        this.validator = validator;
    }

    public async Task<IActionResult> Index()
    {
        return View();
    }

    [HttpGet("Create")]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        var viewModel = await GetViewModel(null, cancellationToken);
        return View(viewModel);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> Edit([FromRoute] long id, CancellationToken cancellationToken)
    {
        var viewModel = await GetViewModel(id, cancellationToken);
        return View("Create", viewModel);
    }

    [HttpPost]
    public async Task<ResultInfo> Post([FromBody] SaveSupplierAddressRequestDto request, CancellationToken cancellationToken)
    {
        var validationResult = await this.validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return new ErrorResultInfo(validationResult.GetErrors());
        }

        await this.supplierAddressService.Save(RequireEntityId, request, cancellationToken);
        return ResultInfo.Ok;
    }

    private async Task<CreateSupplierAddressViewModel> GetViewModel(long? addressId, CancellationToken cancellationToken)
    {
        var address = addressId != null ? await this.supplierAddressService.GetById(addressId.Value, cancellationToken) : null;

        var countries = await this.countryService.GetCountries(cancellationToken);
        var regions = await this.regionService.GetRegions(null, cancellationToken);
        var cities = await this.cityService.GetCities(null, cancellationToken);

        return new CreateSupplierAddressViewModel(
            Address: address,
            Countries: countries,
            Regions: regions,
            Cities: cities
        );
    }
}