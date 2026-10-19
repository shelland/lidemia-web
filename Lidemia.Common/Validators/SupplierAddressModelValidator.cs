// Created on 14/10/2026 22:20 by Laserson

using FluentValidation;
using Lidemia.Core.Models.Domain;

namespace Lidemia.Common.Validators;

public class SupplierAddressModelValidator : AbstractValidator<SaveSupplierAddressRequestDto>
{
    public SupplierAddressModelValidator()
    {
    }
}