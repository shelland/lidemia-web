// Created on 02/09/2026 20:05 by Laserson

using FluentValidation;
using Lidemia.Core.Models.Dto;

namespace Lidemia.Common.Validators;

public class SupplierSignUpModelValidator : AbstractValidator<SupplierSignUpRequestDto>
{
    public SupplierSignUpModelValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithErrorCode("errors.supplier.signup.emailEmpty");
        RuleFor(x => x.Email).MinimumLength(5).WithErrorCode("errors.supplier.signup.emailTooShort");
        RuleFor(x => x.Email).MaximumLength(250).WithErrorCode("errors.supplier.signup.emailTooLong");
        RuleFor(x => x.Email).EmailAddress().WithErrorCode("errors.supplier.signup.emailInvalid");
        RuleFor(x => x.Name).NotEmpty().WithErrorCode("errors.supplier.signup.nameEmpty");
        RuleFor(x => x.Name).MinimumLength(5).WithErrorCode("errors.supplier.signup.nameTooShort");
        RuleFor(x => x.Name).MaximumLength(250).WithErrorCode("errors.supplier.signup.nameTooLong");
        RuleFor(x => x.Phone).NotEmpty().WithErrorCode("errors.supplier.signup.phoneEmpty");
        RuleFor(x => x.Phone).MinimumLength(10).WithErrorCode("errors.supplier.signup.phoneTooShort");
        RuleFor(x => x.Phone).MaximumLength(20).WithErrorCode("errors.supplier.signup.phoneTooLong");
    }
}