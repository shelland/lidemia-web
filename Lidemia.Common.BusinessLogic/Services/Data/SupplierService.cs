// Created on 06/09/2026 14:14 by Laserson

using FluentResults;
using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Common.Mapping;
using Lidemia.Core.Enums;
using Lidemia.Core.Extensions;
using Lidemia.Core.Models.Domain;
using Lidemia.Core.Models.Dto;
using Lidemia.Core.Models.Service;
using Lidemia.DataAccess.Repository.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.Common.BusinessLogic.Services.Data;

[ServiceDescriptor<ISupplierService>(ServiceLifetime.Scoped)]
public class SupplierService : ISupplierService
{
    private readonly ISupplierRepository supplierRepository;
    private readonly IUserService userService;

    public SupplierService(ISupplierRepository supplierRepository, IUserService userService)
    {
        this.supplierRepository = supplierRepository;
        this.userService = userService;
    }

    public async Task<Result<long>> Create(CreateSupplierModel request, CancellationToken cancellationToken)
    {
        return await this.supplierRepository.Create(request, cancellationToken);
    }

    public async Task<SignInResult<SupplierModel?>> SignIn(SupplierSignInRequestDto request, CancellationToken cancellationToken)
    {
        var user = await this.userService.FindUser(request.Email, request.Password, EntityType.Supplier, cancellationToken);

        if (user == null)
        {
            return new SignInResult<SupplierModel?>(Status: LoginResultStatus.NotFound, null);
        }

        var supplier = (await this.supplierRepository.FindSupplierByUserId(user.Id, cancellationToken)).NotNull();
        return new SignInResult<SupplierModel?>(Status: LoginResultStatus.Success, Entity: supplier.ToModel());
    }
}