// Created on 03/09/2026 18:43 by Laserson

using Lidemia.Core.Models.Domain;
using Lidemia.Core.Models.Dto;

namespace Lidemia.Common.BusinessLogic.Services.Data.Abstract;

public interface ISupplierSignInService
{
    Task<SignInResult<SupplierModel?>> SignIn(SupplierSignInRequestDto request, CancellationToken cancellationToken);
}