// Created on 18/09/2026 19:24 by Laserson

using FluentResults;
using Lidemia.Core.Models.Service;
using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Repository.Abstract.Base;

namespace Lidemia.DataAccess.Repository.Abstract;

public interface ISupplierAddressRepository : IRepository<SupplierAddressEntity, long>
{
    Task<Result<long>> Save(SaveSupplierAddressModel model, CancellationToken cancellationToken);
}