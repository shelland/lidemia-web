// Created on 23/11/2026 19:39 by Laserson

using FluentValidation;
using Lidemia.Core.Enums;
using Lidemia.Core.Models.Dto;

namespace Lidemia.Common.Validators;

public class SendChatMessageRequestDtoValidator : AbstractValidator<SendChatMessageRequestDto>
{
    public SendChatMessageRequestDtoValidator()
    {
        RuleFor(x => x.ChatId).GreaterThan(0).WithErrorCode("direct.chat.id.invalid");

        RuleFor(x => x.Text)
            .NotEmpty().WithErrorCode("direct.message.empty")
            .MaximumLength(4000).WithErrorCode("direct.message.tooLong")
            .When(x => x.Type == ChatMessageType.Message);

        RuleFor(x => x.ProductId)
            .NotNull().WithErrorCode("direct.message.product.invalid")
            .GreaterThan(0).WithErrorCode("direct.message.product.invalid")
            .When(x => x.Type == ChatMessageType.Product);

        RuleFor(x => x.OrderId)
            .NotNull().WithErrorCode("direct.message.order.invalid")
            .GreaterThan(0).WithErrorCode("direct.message.order.invalid")
            .When(x => x.Type == ChatMessageType.Order);

        RuleFor(x => x.Type)
            .Must(x => x is ChatMessageType.Message or ChatMessageType.Product or ChatMessageType.Order)
            .WithErrorCode("direct.message.type.unsupported");
    }
}
