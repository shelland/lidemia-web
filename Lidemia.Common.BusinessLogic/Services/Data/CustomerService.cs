// Created on 03/09/2026 18:47 by Laserson

using FluentResults;
using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Common.Mapper;
using Lidemia.Core.Enums;
using Lidemia.Core.Extensions;
using Lidemia.Core.Models.Domain;
using Lidemia.Core.Models.Dto;
using Lidemia.Core.Models.Service;
using Lidemia.DataAccess.Repository.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.Common.BusinessLogic.Services.Data;

[ServiceDescriptor<ICustomerService>(ServiceLifetime.Scoped)]
public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository customerRepository;
    private readonly IUserService userService;

    private readonly ICustomerMapper customerMapper;

    public CustomerService(ICustomerRepository customerRepository, IUserService userService, ICustomerMapper customerMapper)
    {
        this.customerRepository = customerRepository;
        this.userService = userService;
        this.customerMapper = customerMapper;
    }

    public Task<Result<long>> Create(CreateCustomerModel model, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<SignInResult<CustomerModel?>> SignIn(CustomerSignInRequestDto request, CancellationToken cancellationToken)
    {
        var user = await this.userService.FindUser(request.Email, request.Password, EntityType.Customer, cancellationToken);

        if (user == null)
        {
            return new SignInResult<CustomerModel?>(Status: LoginResultStatus.NotFound, null);
        }

        var customer = (await this.customerRepository.FindCustomerByUserId(user.Id, cancellationToken)).NotNull();
        return new SignInResult<CustomerModel?>(LoginResultStatus.Success, this.customerMapper.Map(customer));
    }
}