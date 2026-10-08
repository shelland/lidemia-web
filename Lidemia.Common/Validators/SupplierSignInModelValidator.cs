// Created on 28/09/2026 19:47 by Laserson

using FluentValidation;
using Lidemia.Core.Models.Dto;

namespace Lidemia.Common.Validators;

public class SupplierSignInModelValidator : AbstractValidator<SupplierSignInRequestDto>
{
    public SupplierSignInModelValidator()
    {
    }
}