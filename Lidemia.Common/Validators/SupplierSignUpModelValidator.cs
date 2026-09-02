// Created on 02/09/2026 20:05 by Laserson

using FluentValidation;
using Lidemia.Core.Models.Dto;

namespace Lidemia.Common.Validators;

public class SupplierSignUpModelValidator : AbstractValidator<SupplierSignUpRequestDto>
{
    public SupplierSignUpModelValidator()
    {
    }
}