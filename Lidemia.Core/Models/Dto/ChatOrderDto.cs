// Created on 23/11/2026 21:10 by Laserson

namespace Lidemia.Core.Models.Dto;

/// <summary>
/// An order that is attached to a chat message or offered in the order picker of a chat. Identifiers,
/// money values and statuses are strings because they are rendered as is on the client.
/// </summary>
public record ChatOrderDto
(
    string Id,
    string Number,
    string Status,
    string TotalSum,
    int ItemCount,
    DateTimeOffset CreateDate,
    string? Url
);
