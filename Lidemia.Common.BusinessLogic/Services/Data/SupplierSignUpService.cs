// Created on 03/09/2026 19:01 by Laserson

using FluentResults;
using Lidemia.Common.BusinessLogic.Services.App.Abstract;
using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Core.Models.Dto;
using Lidemia.DataAccess.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Scrutor;

namespace Lidemia.Common.BusinessLogic.Services.Data;

[ServiceDescriptor<ISupplierSignUpService>(ServiceLifetime.Scoped)]
public class SupplierSignUpService : ISupplierSignUpService
{
    private readonly ISupplierService supplierService;
    private readonly ISecurityService securityService;
    private readonly ILogger<SupplierSignUpService> logger;

    public SupplierSignUpService(ISupplierService supplierService, ISecurityService securityService, ILogger<SupplierSignUpService> logger)
    {
        this.supplierService = supplierService;
        this.securityService = securityService;
        this.logger = logger;
    }

    public async Task<Result<long>> SignUp(SupplierSignUpRequestDto request, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Registering a new supplier {Name} ({Email})", request.FullName, request.Email);

        var hashedPassword = this.securityService.HashPassword(request.Password);

        var result = await this.supplierService.Create(new CreateSupplierModel(
                Email: request.Email,
                HashedPassword: hashedPassword.Hash,
                PasswordSalt: hashedPassword.Salt),
            cancellationToken);

        this.logger.LogInformation("Created a new supplier {Email} with ID: {Id}", request.Email, result.Value);

        return result;
    }
}