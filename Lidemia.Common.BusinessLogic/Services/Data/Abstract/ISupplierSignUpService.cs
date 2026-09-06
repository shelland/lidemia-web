// Created on 03/09/2026 18:43 by Laserson

using FluentResults;
using Lidemia.Core.Models.Dto;

namespace Lidemia.Common.BusinessLogic.Services.Data.Abstract;

public interface ISupplierSignUpService
{
    Task<Result<long>> SignUp(SupplierSignUpRequestDto request, CancellationToken cancellationToken);
}