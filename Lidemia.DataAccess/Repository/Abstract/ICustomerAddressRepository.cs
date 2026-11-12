// Created on 18/09/2026 19:24 by Laserson

using FluentResults;
using Lidemia.Core.Models.Misc;
using Lidemia.Core.Models.Service;
using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Repository.Abstract.Base;
using X.PagedList;

namespace Lidemia.DataAccess.Repository.Abstract;

public interface ICustomerAddressRepository : IRepository<CustomerAddressEntity, long>
{
    Task<IPagedList<CustomerAddressEntity>> GetCustomerAddresses(long customerId, PagingInfoModel pagingInfo, CancellationToken cancellationToken);

    Task<Result<long>> Save(SaveCustomerAddressModel model, CancellationToken cancellationToken);
}