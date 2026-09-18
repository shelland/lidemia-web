// Created on 03/09/2026 19:00 by Laserson

using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Core.Models.Domain;
using Lidemia.Core.Models.Dto;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.Common.BusinessLogic.Services.Data;

[ServiceDescriptor<ISupplierSignInService>(ServiceLifetime.Scoped)]
public class SupplierSignInService : ISupplierSignInService
{
    private readonly ISupplierService supplierService;

    public SupplierSignInService(ISupplierService supplierService)
    {
        this.supplierService = supplierService;
    }

    public Task<SignInResult<SupplierModel?>> SignIn(SupplierSignInRequestDto request, CancellationToken cancellationToken)
    {
        return this.supplierService.SignIn(request, cancellationToken);
    }
}