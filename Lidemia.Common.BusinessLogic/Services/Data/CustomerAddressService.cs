// Created on 02/10/2026 18:56 by Laserson

using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.DataAccess.Repository.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.Common.BusinessLogic.Services.Data;

[ServiceDescriptor<ICustomerAddressService>(ServiceLifetime.Scoped)]
public class CustomerAddressService : ICustomerAddressService
{
    private readonly ICustomerAddressRepository customerAddressRepository;

    public CustomerAddressService(ICustomerAddressRepository customerAddressRepository)
    {
        this.customerAddressRepository = customerAddressRepository;
    }
}