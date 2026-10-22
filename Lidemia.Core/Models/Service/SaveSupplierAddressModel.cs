// Created on 14/10/2026 22:06 by Laserson

using Lidemia.Core.Enums;

namespace Lidemia.Core.Models.Service;

public record SaveSupplierAddressModel
(
    long SupplierId,
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