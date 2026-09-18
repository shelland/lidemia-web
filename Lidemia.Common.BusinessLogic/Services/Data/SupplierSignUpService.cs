// Created on 03/09/2026 19:01 by Laserson

using FluentResults;
using Lidemia.Common.BusinessLogic.Services.App.Abstract;
using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Common.Extensions;
using Lidemia.Core.Models.Bus;
using Lidemia.Core.Models.Dto;
using Lidemia.Core.Models.Service;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Scrutor;

namespace Lidemia.Common.BusinessLogic.Services.Data;

[ServiceDescriptor<ISupplierSignUpService>(ServiceLifetime.Scoped)]
public class SupplierSignUpService : ISupplierSignUpService
{
    private readonly ISupplierService supplierService;
    private readonly ISecurityService securityService;
    private readonly SupplierMetrics metrics;
    private readonly IPublishEndpoint publishEndpoint;
    private readonly ILogger<SupplierSignUpService> logger;

    public SupplierSignUpService(ISupplierService supplierService, ISecurityService securityService, SupplierMetrics metrics,
        ILogger<SupplierSignUpService> logger, IPublishEndpoint publishEndpoint)
    {
        this.supplierService = supplierService;
        this.securityService = securityService;
        this.metrics = metrics;
        this.logger = logger;
        this.publishEndpoint = publishEndpoint;
    }

    public async Task<Result<long>> SignUp(SupplierSignUpRequestDto request, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Registering a new supplier {Name} ({Email})", request.Name, request.Email);

        var hashedPassword = this.securityService.HashPassword(request.Password);

        var result = await this.supplierService.Create(new CreateSupplierModel(
                Email: request.Email,
                Name: request.Name,
                HashedPassword: hashedPassword.Hash,
                PasswordSalt: hashedPassword.Salt,
                Inn: request.Inn,
                Ogrn: request.Ogrn,
                OrganizationType: request.Type
            ),
            cancellationToken);

        this.metrics.OnNewSupplier();
        await this.publishEndpoint.Publish(new SupplierSignUpBusEventModel(Id: result.Value), cancellationToken);

        this.logger.LogInformation("Created a new supplier {Email} with ID: {Id}", request.Email, result.Value);

        return result;
    }
}