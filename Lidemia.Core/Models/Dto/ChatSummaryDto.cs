// Created on 23/11/2026 20:05 by Laserson

namespace Lidemia.Core.Models.Dto;

/// <summary>
/// A chat list entry that is pushed to the participants in realtime. Both participant names are sent
/// so each side can display the counterparty without an extra request.
/// </summary>
public record ChatSummaryDto
(
    string ChatId,
    string CustomerId,
    string SupplierId,
    string CustomerName,
    string SupplierName,
    string? LastMessageText,
    DateTimeOffset? LastMessageDate
);
