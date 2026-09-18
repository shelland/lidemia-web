// Created on 02/09/2026 20:05 by Laserson

using FluentValidation;
using Lidemia.Core.Models.Dto;

namespace Lidemia.Common.Validators;

public class CustomerSignUpModelValidator : AbstractValidator<CustomerSignInRequestDto>
{
    public CustomerSignUpModelValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithErrorCode("errors.customer.signup.emailEmpty");
    }
}