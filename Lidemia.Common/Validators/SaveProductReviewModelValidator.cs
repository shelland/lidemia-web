// Created on 31/10/2026 14:37 by Laserson

using FluentValidation;
using Lidemia.Core.Models.Dto;

namespace Lidemia.Common.Validators;

public class SaveProductReviewModelValidator : AbstractValidator<SaveProductReviewRequestDto>
{
    public SaveProductReviewModelValidator()
    {
        RuleFor(x => x.Review).NotEmpty().WithErrorCode("productReview.review.isEmpty");
        RuleFor(x => x.Review).MaximumLength(5000).WithErrorCode("productReview.review.tooLong");
        RuleFor(x => x.PositiveReview).MaximumLength(5000).WithErrorCode("productReview.positiveReview.tooLong");
        RuleFor(x => x.NegativeReview).MaximumLength(5000).WithErrorCode("productReview.negativeReview.tooLong");
    }
}