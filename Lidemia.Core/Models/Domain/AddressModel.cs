// Created on 12/10/2026 22:38 by Laserson

using Lidemia.Core.Models.Base;

namespace Lidemia.Core.Models.Domain;

public class AddressModel : AbstractEntity<long>
{
    public CountryModel Country { get; set; } = null!;

    public RegionModel Region { get; set; } = null!;

    public CityModel City { get; set; } = null!;

    public string StreetName { get; set; } = string.Empty;

    public string HouseNumber { get; set; } = string.Empty;

    public string FlatNumber { get; set; } = string.Empty;

    public string? HouseAdd1 { get; set; }

    public string? HouseAdd2 { get; set; }

    public string? AddressLine1 { get; set; }

    public string? AddressLine2 { get; set; }

}