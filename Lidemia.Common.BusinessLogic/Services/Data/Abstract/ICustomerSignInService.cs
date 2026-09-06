// Created on 03/09/2026 18:42 by Laserson

using Lidemia.Core.Models.Domain;
using Lidemia.Core.Models.Dto;

namespace Lidemia.Common.BusinessLogic.Services.Data.Abstract;

public interface ICustomerSignInService
{
    Task<SignInResult<CustomerModel?>> SignIn(CustomerSignInRequestDto request, CancellationToken cancellationToken);
}