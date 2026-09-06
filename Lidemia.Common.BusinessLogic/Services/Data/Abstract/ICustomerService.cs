// Created on 02/09/2026 20:09 by Laserson

using Lidemia.Core.Models.Domain;
using Lidemia.Core.Models.Dto;

namespace Lidemia.Common.BusinessLogic.Services.Data.Abstract;

public interface ICustomerService
{
    Task<SignInResult<CustomerModel?>> SignIn(CustomerSignInRequestDto request, CancellationToken cancellationToken);
}