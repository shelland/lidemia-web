// Created on 10/11/2026 17:18 by Laserson

using FluentValidation;
using Lidemia.Core.Models.Dto;

namespace Lidemia.Common.Validators;

public class CustomerAddressModelValidator : AbstractValidator<SaveCustomerAddressRequestDto>
{
    public CustomerAddressModelValidator()
    {
    }
}