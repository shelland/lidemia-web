// Created on 26/10/2026 19:54 by Laserson

using Lidemia.Common.Mapper.Abstract;
using Lidemia.Core.Models.Domain;
using Lidemia.Core.Models.Misc;
using Lidemia.DataAccess.Entities;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.Common.Mapper;

public interface ICustomerMapper : IMapper<CustomerEntity, CustomerModel>;

public interface ILightCustomerMapper : IMapper<CustomerEntity, LightCustomerModel>;

[ServiceDescriptor<ICustomerMapper>(ServiceLifetime.Scoped)]
public class CustomerMapper : ICustomerMapper
{
    private readonly IUserMapper userMapper;

    public CustomerMapper(IUserMapper userMapper)
    {
        this.userMapper = userMapper;
    }

    public CustomerModel Map(CustomerEntity source)
    {
        return new CustomerModel
        {
            Id = source.Id,
            User = this.userMapper.Map(source.User)
        };
    }
}

[ServiceDescriptor<ILightCustomerMapper>(ServiceLifetime.Scoped)]
public class LightCustomerMapper : ILightCustomerMapper
{
    public LightCustomerModel Map(CustomerEntity source)
    {
        return new LightCustomerModel(Id: source.Id, FirstName: source.FirstName, LastName: source.LastName);
    }
}