// Created on 02/09/2026 20:09 by Laserson

using FluentResults;
using Lidemia.Core.Models.Domain;
using Lidemia.Core.Models.Dto;
using Lidemia.Core.Models.Service;

namespace Lidemia.Common.BusinessLogic.Services.Data.Abstract;

public interface ISupplierService
{
    Task<Result<long>> Create(CreateSupplierModel request, CancellationToken cancellationToken);

    Task<SignInResult<SupplierModel?>> SignIn(SupplierSignInRequestDto request, CancellationToken cancellationToken);
}