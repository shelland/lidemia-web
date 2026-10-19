// Created on 13/10/2026 19:04 by Laserson

using Lidemia.Core.Models.Domain;

namespace Lidemia.ViewModels
{
    public record CreateSupplierAddressViewModel
    (
        SupplierAddressModel? Address,
        IEnumerable<CountryModel> Countries,
        IEnumerable<RegionModel> Regions,
        IEnumerable<CityModel> Cities
    );
}