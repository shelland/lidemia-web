// Created on 23/11/2026 20:35 by Laserson

using Lidemia.Core.Enums;

namespace Lidemia.Core.Models.Dto;

public record SendChatMessageRequestDto
(
    long ChatId,
    ChatMessageType Type,
    string? Text,
    long? ProductId,
    long? OrderId
);
