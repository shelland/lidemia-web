// Created on 01/10/2026 19:20 by Laserson

using FluentValidation;
using Lidemia.Core.Models.Dto;

namespace Lidemia.Common.Validators;

public class SaveProductModelValidator : AbstractValidator<SaveProductRequestDto>
{
    public SaveProductModelValidator()
    {
        RuleFor(x => x.Title).MaximumLength(250).WithErrorCode("errors.product.titleTooLong");
    }
}