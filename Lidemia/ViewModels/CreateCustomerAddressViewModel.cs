// Created on 10/11/2026 16:23 by Laserson

using Lidemia.Core.Models.Domain;

namespace Lidemia.ViewModels;

public record CreateCustomerAddressViewModel
(
    CustomerAddressModel? Address,
    IEnumerable<CountryModel> Countries,
    IEnumerable<RegionModel> Regions,
    IEnumerable<CityModel> Cities
);