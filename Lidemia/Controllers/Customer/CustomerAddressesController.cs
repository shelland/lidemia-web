// Created on 02/10/2026 18:55 by Laserson

using FluentValidation;
using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Core.Attributes;
using Lidemia.Core.Extensions;
using Lidemia.Core.Models.Dto;
using Lidemia.Core.Models.Misc;
using Lidemia.Core.Models.Service;
using Lidemia.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Lidemia.Controllers.Customer;

[CustomerAuthorize]
[Route("Customer/Addresses")]
public class CustomerAddressesController : BaseController
{
    private readonly ICustomerAddressService customerAddressService;
    private readonly ICountryService countryService;
    private readonly IRegionService regionService;
    private readonly ICityService cityService;
    private readonly IValidator<SaveCustomerAddressRequestDto> validator;

    public CustomerAddressesController(
        ICustomerAddressService customerAddressService,
        IValidator<SaveCustomerAddressRequestDto> validator,
        ICountryService countryService,
        IRegionService regionService,
        ICityService cityService)
    {
        this.customerAddressService = customerAddressService;
        this.validator = validator;
        this.countryService = countryService;
        this.regionService = regionService;
        this.cityService = cityService;
    }

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] PagingInfoModel pagingInfo, CancellationToken cancellationToken)
    {
        var addresses = await this.customerAddressService.GetCustomerAddresses(RequireEntityId, pagingInfo, cancellationToken);
        return View(addresses);
    }

    [HttpGet("Create")]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        var viewModel = await GetViewModel(null, cancellationToken);
        return View(viewModel);
    }

    [HttpGet("Edit/{id:long}")]
    public async Task<IActionResult> Edit([FromRoute] long id, CancellationToken cancellationToken)
    {
        var viewModel = await GetViewModel(id, cancellationToken);
        return View("Create", viewModel);
    }

    [HttpPost]
    public async Task<ResultInfo> Post([FromBody] SaveCustomerAddressRequestDto request, CancellationToken cancellationToken)
    {
        var validationResult = await this.validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return new ErrorResultInfo(validationResult.GetErrors());
        }

        var result = await this.customerAddressService.Save(RequireEntityId, request, cancellationToken);
        return ResultInfo.Ok;
    }

    private async Task<CreateCustomerAddressViewModel> GetViewModel(long? addressId, CancellationToken cancellationToken)
    {
        var address = addressId != null ? await this.customerAddressService.GetById(addressId.Value, cancellationToken) : null;

        var countries = await this.countryService.GetCountries(cancellationToken);
        var regions = await this.regionService.GetRegions(null, cancellationToken);
        var cities = await this.cityService.GetCities(null, cancellationToken);

        return new CreateCustomerAddressViewModel(
            Address: address,
            Countries: countries,
            Regions: regions,
            Cities: cities
        );
    }
}