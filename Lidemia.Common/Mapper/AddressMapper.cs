// Created on 26/10/2026 19:43 by Laserson

using Lidemia.Common.Mapper.Abstract;
using Lidemia.Core.Extensions;
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
    private readonly ICountryMapper countryMapper;
    private readonly IRegionMapper regionMapper;
    private readonly ICityMapper cityMapper;

    public AddressMapper(ICountryMapper countryMapper, IRegionMapper regionMapper, ICityMapper cityMapper)
    {
        this.countryMapper = countryMapper;
        this.regionMapper = regionMapper;
        this.cityMapper = cityMapper;
    }

    public AddressModel? Map(AddressEntity? source)
    {
        if (source == null)
        {
            return null;
        }

        return new AddressModel
        {
            Id = source.NotNull().Id,
            CreateDate = source.CreateDate,
            Region = this.regionMapper.Map(source.Region).NotNull(),
            City = this.cityMapper.Map(source.City).NotNull(),
            Country = this.countryMapper.Map(source.Country).NotNull(),
            StreetName = source.StreetName,
            HouseNumber = source.HouseNumber,
            HouseAdd1 = source.HouseAdd1,
            HouseAdd2 = source.HouseAdd2,
            AddressLine1 = source.AddressLine1,
            AddressLine2 = source.AddressLine2
        };
    }
}

[ServiceDescriptor<ICustomerAddressMapper>(ServiceLifetime.Scoped)]
public class CustomerAddressMapper : ICustomerAddressMapper
{
    private readonly IAddressMapper addressMapper;

    public CustomerAddressMapper(IAddressMapper addressMapper)
    {
        this.addressMapper = addressMapper;
    }

    public CustomerAddressModel? Map(CustomerAddressEntity? source)
    {
        if (source == null)
        {
            return null;
        }

        var address = this.addressMapper.Map(source.Address).NotNull();

        return new CustomerAddressModel
        {
            Id = source.Id,
            CreateDate = source.CreateDate,
            AddressLine1 = address.AddressLine1,
            AddressLine2 = address.AddressLine2,
            City = address.City,
            Comment = source.Comment,
            Region = address.Region,
            Country = address.Country,
            FlatNumber = address.FlatNumber,
            HouseNumber = address.HouseNumber,
            StreetName = address.StreetName,
            CustomerId = source.CustomerId
        };
    }
}

[ServiceDescriptor<ISupplierAddressMapper>(ServiceLifetime.Scoped)]
public class SupplierAddressMapper : ISupplierAddressMapper
{
    private readonly IAddressMapper addressMapper;

    public SupplierAddressMapper(IAddressMapper addressMapper)
    {
        this.addressMapper = addressMapper;
    }

    public SupplierAddressModel? Map(SupplierAddressEntity? source)
    {
        if (source == null)
        {
            return null;
        }

        var address = this.addressMapper.Map(source.Address).NotNull();

        return new SupplierAddressModel
        {
            Id = source.Id,
            CreateDate = source.CreateDate,
            AddressLine1 = address.AddressLine1,
            AddressLine2 = address.AddressLine2,
            City = address.City,
            Comment = source.Comment,
            Region = address.Region,
            Country = address.Country,
            FlatNumber = address.FlatNumber,
            HouseNumber = address.HouseNumber,
            StreetName = address.StreetName,
            SupplierId = source.SupplierId
        };
    }
}

[ServiceDescriptor<ICountryMapper>(ServiceLifetime.Scoped)]
public class CountryMapper : ICountryMapper
{
    public CountryModel? Map(CountryEntity? source)
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
    public RegionModel? Map(RegionEntity? source)
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
    public CityModel? Map(CityEntity? source)
    {
        return new CityModel
        {
            Id = source.Id,
            Name = source.Name,
            RegionId = source.RegionId
        };
    }
}