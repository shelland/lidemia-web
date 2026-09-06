// Created on 03/09/2026 18:47 by Laserson

using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Core.Models.Domain;
using Lidemia.Core.Models.Dto;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.Common.BusinessLogic.Services.Data;

[ServiceDescriptor<ICustomerSignInService>(ServiceLifetime.Scoped)]
public class CustomerSignInService : ICustomerSignInService
{
    private readonly ICustomerService customerService;

    public CustomerSignInService(ICustomerService customerService)
    {
        this.customerService = customerService;
    }

    public Task<SignInResult<CustomerModel?>> SignIn(CustomerSignInRequestDto request, CancellationToken cancellationToken)
    {
        return this.customerService.SignIn(request, cancellationToken);
    }
}