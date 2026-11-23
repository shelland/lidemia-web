// Created on 23/11/2026 19:37 by Laserson

namespace Lidemia.Core.Models.Dto;

/// <summary>
/// Payload that is pushed to the chat participants in realtime. Identifiers are strings because the
/// application serializes 64-bit integers as strings to keep JavaScript precision.
/// </summary>
public record ChatMessageDto
(
    string Id,
    string ChatId,
    string CustomerId,
    string SupplierId,
    string SenderId,
    string SenderType,
    string Type,
    string? Text,
    ChatProductDto? Product,
    ChatOrderDto? Order,
    DateTimeOffset CreateDate
);
