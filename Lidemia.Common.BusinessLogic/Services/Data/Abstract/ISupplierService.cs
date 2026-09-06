// Created on 02/09/2026 20:09 by Laserson

using FluentResults;
using Lidemia.DataAccess.Models;

namespace Lidemia.Common.BusinessLogic.Services.Data.Abstract;

public interface ISupplierService
{
    Task<Result<long>> Create(CreateSupplierModel request, CancellationToken cancellationToken);
}