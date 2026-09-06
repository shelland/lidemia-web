// Created on 03/09/2026 18:47 by Laserson

using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Common.Mapping;
using Lidemia.Core.Enums;
using Lidemia.Core.Extensions;
using Lidemia.Core.Models.Domain;
using Lidemia.Core.Models.Dto;
using Lidemia.DataAccess.Repository.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.Common.BusinessLogic.Services.Data;

[ServiceDescriptor<ICustomerService>(ServiceLifetime.Scoped)]
public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository customerRepository;
    private readonly IUserService userService;

    public CustomerService(ICustomerRepository customerRepository, IUserService userService)
    {
        this.customerRepository = customerRepository;
        this.userService = userService;
    }

    public async Task<SignInResult<CustomerModel?>> SignIn(CustomerSignInRequestDto request, CancellationToken cancellationToken)
    {
        var user = await this.userService.FindUser(request.Email, request.Password, EntityType.Customer, cancellationToken);

        if (user == null)
        {
            return new SignInResult<CustomerModel?>(Status: LoginResultStatus.NotFound, null);
        }

        var customer = (await this.customerRepository.FindCustomerByUserId(user.Id, cancellationToken)).NotNull();
        return new SignInResult<CustomerModel?>(LoginResultStatus.Success, customer.ToModel());
    }
}