// Created on 26/10/2026 19:43 by Laserson

using Lidemia.Common.Mapper.Abstract;
using Lidemia.Core.Models.Domain;
using Lidemia.DataAccess.Entities;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.Common.Mapper;

public interface IAddressMapper : IMapper<AddressEntity, AddressModel>;

public interface ICustomerAddressMapper : IMapper<CustomerAddressEntity, CustomerAddressModel>;

public interface ISupplierAddressMapper : IMapper<SupplierAddressEntity, SupplierAddressModel>;

public interface ICountryMapper : IMapper<CountryEntity, CountryModel>;

public interface IRegionMapper : IMapper<RegionEntity, RegionModel>;

public interface ICityMapper : IMapper<CityEntity, CityModel>;

[ServiceDescriptor<IAddressMapper>(ServiceLifetime.Scoped)]
public class AddressMapper : IAddressMapper
{
    public AddressModel Map(AddressEntity source)
    {
        return new AddressModel
        {
            Id = source.Id
        };
    }
}

[ServiceDescriptor<ICustomerAddressMapper>(ServiceLifetime.Scoped)]
public class CustomerAddressMapper : ICustomerAddressMapper
{
    public CustomerAddressModel Map(CustomerAddressEntity source)
    {
        return new CustomerAddressModel
        {
            Id = source.Id
        };
    }
}

[ServiceDescriptor<ISupplierAddressMapper>(ServiceLifetime.Scoped)]
public class SupplierAddressMapper : ISupplierAddressMapper
{
    public SupplierAddressModel Map(SupplierAddressEntity source)
    {
        return new SupplierAddressModel
        {
            Id = source.Id
        };
    }
}

[ServiceDescriptor<ICountryMapper>(ServiceLifetime.Scoped)]
public class CountryMapper : ICountryMapper
{
    public CountryModel Map(CountryEntity source)
    {
        return new CountryModel
        {
            Id = source.Id,
            Name = source.Name
        };
    }
}

[ServiceDescriptor<IRegionMapper>(ServiceLifetime.Scoped)]
public class RegionMapper : IRegionMapper
{
    public RegionModel Map(RegionEntity source)
    {
        return new RegionModel
        {
            Id = source.Id,
            Name = source.Name,
            CountryId = source.CountryId
        };
    }
}

[ServiceDescriptor<ICityMapper>(ServiceLifetime.Scoped)]
public class CityMapper : ICityMapper
{
    public CityModel Map(CityEntity source)
    {
        return new CityModel
        {
            Id = source.Id,
            Name = source.Name,
            RegionId = source.RegionId
        };
    }
}