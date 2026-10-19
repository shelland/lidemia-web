// Created on 12/10/2026 22:40 by Laserson

using Lidemia.Core.Models.Domain;
using Lidemia.DataAccess.Entities;

namespace Lidemia.Common.Mapping;

public static class AddressMapping
{
    public static AddressModel ToModel(this AddressEntity entity)
    {
        return new AddressModel
        {
            Id = entity.Id
        };
    }

    public static SupplierAddressModel ToModel(this SupplierAddressEntity entity)
    {
        return new SupplierAddressModel
        {
            Id = entity.Id
        };
    }

    public static CustomerAddressModel ToModel(this CustomerAddressEntity entity)
    {
        return new CustomerAddressModel
        {
            Id = entity.Id
        };
    }

    public static CountryModel ToModel(this CountryEntity entity)
    {
        return new CountryModel
        {
            Id = entity.Id,
            Name = entity.Name
        };
    }

    public static RegionModel ToModel(this RegionEntity entity)
    {
        return new RegionModel
        {
            Id = entity.Id,
            Name = entity.Name,
            CountryId = entity.CountryId
        };
    }

    public static CityModel ToModel(this CityEntity entity)
    {
        return new CityModel
        {
            Id = entity.Id,
            Name = entity.Name,
            RegionId = entity.RegionId
        };
    }
}