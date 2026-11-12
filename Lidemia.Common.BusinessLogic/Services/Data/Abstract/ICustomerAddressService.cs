// Created on 02/10/2026 18:56 by Laserson

using FluentResults;
using Lidemia.Core.Models.Domain;
using Lidemia.Core.Models.Dto;
using Lidemia.Core.Models.Misc;

namespace Lidemia.Common.BusinessLogic.Services.Data.Abstract;

public interface ICustomerAddressService
{
    Task<CustomerAddressModel?> GetById(long id, CancellationToken cancellationToken);

    Task<BasePagedListModel<CustomerAddressModel>> GetCustomerAddresses(long customerId, PagingInfoModel pagingInfo, CancellationToken cancellationToken);

    Task<Result<long>> Save(long customerId, SaveCustomerAddressRequestDto request, CancellationToken cancellationToken);
}