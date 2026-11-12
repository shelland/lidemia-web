// Created on 10/11/2026 16:08 by Laserson

namespace Lidemia.Core.Models.Dto;

public record SaveCustomerAddressRequestDto
(
    long? Id,
    long CountryId,
    long RegionId,
    long CityId,
    string StreetName,
    string HouseNumber,
    string FlatNumber,
    string Comment
);