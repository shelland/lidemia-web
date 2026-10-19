// Created on 03/09/2026 18:47 by Laserson

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

[ServiceDescriptor<ICustomerSignUpService>(ServiceLifetime.Scoped)]
public class CustomerSignUpService : ICustomerSignUpService
{
    private readonly ICustomerService customerService;
    private readonly ISecurityService securityService;
    private readonly CustomerMetrics customerMetrics;
    private readonly IPublishEndpoint publishEndpoint;
    private readonly ILogger<CustomerSignUpService> logger;

    public CustomerSignUpService(
        ICustomerService customerService,
        ISecurityService securityService,
        CustomerMetrics customerMetrics,
        IPublishEndpoint publishEndpoint,
        ILogger<CustomerSignUpService> logger)
    {
        this.customerService = customerService;
        this.securityService = securityService;
        this.customerMetrics = customerMetrics;
        this.publishEndpoint = publishEndpoint;
        this.logger = logger;
    }

    public async Task<Result<long>> SignUp(CustomerSignUpRequestDto request, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Registering a new customer {Email}", request.Email);

        var hashedPassword = this.securityService.HashPassword(request.Password);
        var result = await this.customerService.Create(new CreateCustomerModel(
            Email: request.Email,
            HashedPassword: hashedPassword.Hash,
            PasswordSalt: hashedPassword.Salt,
            FirstName: request.FirstName,
            LastName: request.LastName), cancellationToken);

        this.customerMetrics.OnNewCustomer();
        await this.publishEndpoint.Publish(new CustomerSignUpBusEventModel(Id: result.Value), cancellationToken);

        this.logger.LogInformation("A new customer {Email} was registered", request.Email);

        return result;
    }
}