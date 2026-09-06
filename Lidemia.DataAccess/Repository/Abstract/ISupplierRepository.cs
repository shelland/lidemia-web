// Created on 01/09/2026 15:43 by Laserson

using FluentResults;
using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Models;
using Lidemia.DataAccess.Repository.Abstract.Base;

namespace Lidemia.DataAccess.Repository.Abstract;

public interface ISupplierRepository : IRepository<SupplierEntity, long>
{
    Task<Result<long>> Create(CreateSupplierModel model, CancellationToken cancellationToken);
}