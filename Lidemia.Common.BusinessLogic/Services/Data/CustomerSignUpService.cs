// Created on 03/09/2026 18:47 by Laserson

using FluentResults;
using Lidemia.Common.BusinessLogic.Services.App.Abstract;
using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Core.Models.Dto;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.Common.BusinessLogic.Services.Data;

[ServiceDescriptor<ICustomerSignUpService>(ServiceLifetime.Scoped)]
public class CustomerSignUpService : ICustomerSignUpService
{
    private readonly ICustomerService customerService;
    private readonly ISecurityService securityService;

    public CustomerSignUpService(ICustomerService customerService, ISecurityService securityService)
    {
        this.customerService = customerService;
        this.securityService = securityService;
    }

    public Task<Result<long>> SignUp(CustomerSignUpRequestDto request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}