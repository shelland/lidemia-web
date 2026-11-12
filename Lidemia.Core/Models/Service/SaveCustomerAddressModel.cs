// Created on 10/11/2026 16:13 by Laserson

namespace Lidemia.Core.Models.Service;

public record SaveCustomerAddressModel
(
    long? Id,
    long CustomerId,
    long CountryId,
    long RegionId,
    long CityId,
    string StreetName,
    string HouseNumber,
    string FlatNumber,
    string Comment
);