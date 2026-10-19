// Created on 14/10/2026 22:06 by Laserson

using Lidemia.Core.Enums;

namespace Lidemia.Core.Models.Service;

public record SaveSupplierAddressModel
(
    long SupplierId,
    Guid? Id,
    Guid CountryId,
    Guid RegionId,
    Guid CityId,
    string StreetName,
    string HouseNumber,
    string FlatNumber,
    string Comment,
    SupplierAddressType? Type
);