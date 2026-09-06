// Created on 06/09/2026 14:14 by Laserson

using FluentResults;
using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Core.Models.Dto;
using Lidemia.DataAccess.Models;
using Lidemia.DataAccess.Repository.Abstract;

namespace Lidemia.Common.BusinessLogic.Services.Data;

public class SupplierService : ISupplierService
{
    private readonly ISupplierRepository supplierRepository;

    public SupplierService(ISupplierRepository supplierRepository)
    {
        this.supplierRepository = supplierRepository;
    }

    public async Task<Result<long>> Create(CreateSupplierModel request, CancellationToken cancellationToken)
    {
        return await this.supplierRepository.Create(request, cancellationToken);
    }
}