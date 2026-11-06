// Created on 05/11/2026 21:24 by Laserson

using FluentValidation;
using Lidemia.Core.Models.Dto;

namespace Lidemia.Common.Validators;

public class CustomerSignInModelValidator : AbstractValidator<CustomerSignInRequestDto>
{
    public CustomerSignInModelValidator()
    {
    }
}