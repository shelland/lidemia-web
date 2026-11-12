// Created on 02/10/2026 18:56 by Laserson

using FluentResults;
using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Common.Mapper;
using Lidemia.Common.Mapper.Abstract;
using Lidemia.Core.Models.Domain;
using Lidemia.Core.Models.Dto;
using Lidemia.Core.Models.Misc;
using Lidemia.Core.Models.Service;
using Lidemia.DataAccess.Repository.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.Common.BusinessLogic.Services.Data;

[ServiceDescriptor<ICustomerAddressService>(ServiceLifetime.Scoped)]
public class CustomerAddressService : ICustomerAddressService
{
    private readonly ICustomerAddressRepository customerAddressRepository;
    private readonly ICustomerAddressMapper customerAddressMapper;

    public CustomerAddressService(ICustomerAddressRepository customerAddressRepository, ICustomerAddressMapper customerAddressMapper)
    {
        this.customerAddressRepository = customerAddressRepository;
        this.customerAddressMapper = customerAddressMapper;
    }

    public async Task<CustomerAddressModel?> GetById(long id, CancellationToken cancellationToken)
    {
        var address = await this.customerAddressRepository.GetById(id, cancellationToken);
        return address != null ? this.customerAddressMapper.Map(address) : null;
    }

    public async Task<BasePagedListModel<CustomerAddressModel>> GetCustomerAddresses(long customerId, PagingInfoModel pagingInfo,
        CancellationToken cancellationToken)
    {
        var addresses = await this.customerAddressRepository.GetCustomerAddresses(customerId, pagingInfo, cancellationToken);

        return new BasePagedListModel<CustomerAddressModel>
        {
            CurrentPage = pagingInfo.Page,
            TotalCount = addresses.TotalItemCount,
            TotalPages = addresses.PageCount,
            Items = this.customerAddressMapper.MapList(addresses)
        };
    }

    public Task<Result<long>> Save(long customerId, SaveCustomerAddressRequestDto request, CancellationToken cancellationToken)
    {
        return this.customerAddressRepository.Save(
            new SaveCustomerAddressModel(
                Id: request.Id,
                CustomerId: customerId,
                CountryId: request.CountryId,
                RegionId: request.RegionId,
                CityId: request.CityId,
                StreetName: request.StreetName,
                HouseNumber: request.HouseNumber,
                FlatNumber: request.FlatNumber,
                Comment: request.Comment), cancellationToken);
    }
}