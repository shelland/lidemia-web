// Created on 23/11/2026 20:35 by Laserson

namespace Lidemia.Core.Models.Dto;

/// <summary>
/// A product that is attached to a chat message or offered in the product picker of a chat.
/// Identifiers are strings because the application serializes 64-bit integers as strings to keep
/// JavaScript precision.
/// </summary>
public record ChatProductDto
(
    string Id,
    string? Title,
    string? Price,
    string? PhotoUrl,
    string? Url
);
