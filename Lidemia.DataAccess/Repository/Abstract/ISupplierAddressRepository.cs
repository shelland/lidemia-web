// Created on 18/09/2026 19:24 by Laserson

using FluentResults;
using Lidemia.Core.Models.Misc;
using Lidemia.Core.Models.Service;
using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Repository.Abstract.Base;
using X.PagedList;

namespace Lidemia.DataAccess.Repository.Abstract;

public interface ISupplierAddressRepository : IRepository<SupplierAddressEntity, long>
{
    Task<Result<long>> Save(SaveSupplierAddressModel model, CancellationToken cancellationToken);

    Task<IPagedList<SupplierAddressEntity>> GetSupplierAddresses(long supplierId, PagingInfoModel pagingInfo, CancellationToken cancellationToken);

    Task<IEnumerable<SupplierAddressEntity>> GetAddresses(IEnumerable<long> supplierIds, CancellationToken cancellationToken);
}