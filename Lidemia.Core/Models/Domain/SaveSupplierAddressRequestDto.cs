// Created on 14/10/2026 22:00 by Laserson

using Lidemia.Core.Enums;

namespace Lidemia.Core.Models.Domain;

public record SaveSupplierAddressRequestDto
(
    long? Id,
    long CountryId,
    long RegionId,
    long CityId,
    string StreetName,
    string HouseNumber,
    string FlatNumber,
    string Comment,
    SupplierAddressType? Type
);