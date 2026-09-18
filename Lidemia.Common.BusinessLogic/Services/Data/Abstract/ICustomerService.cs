// Created on 02/09/2026 20:09 by Laserson

using FluentResults;
using Lidemia.Core.Models.Domain;
using Lidemia.Core.Models.Dto;
using Lidemia.Core.Models.Service;

namespace Lidemia.Common.BusinessLogic.Services.Data.Abstract;

public interface ICustomerService
{
    Task<Result<long>> Create(CreateCustomerModel model, CancellationToken cancellationToken);

    Task<SignInResult<CustomerModel?>> SignIn(CustomerSignInRequestDto request, CancellationToken cancellationToken);
}