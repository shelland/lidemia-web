// Created on 11/10/2026 15:31 by Laserson

using FluentResults;
using Lidemia.Core.Models.Domain;
using Lidemia.Core.Models.Misc;

namespace Lidemia.Common.BusinessLogic.Services.Data.Abstract;

public interface ISupplierAddressService
{
    Task<BasePagedListModel<SupplierAddressModel>> GetAddresses();

    Task<SupplierAddressModel?> GetById(long id, CancellationToken cancellationToken);

    Task<Result<long>> Save(long supplierId, SaveSupplierAddressRequestDto request, CancellationToken cancellationToken);
}