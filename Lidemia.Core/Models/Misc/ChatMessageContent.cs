// Created on 23/11/2026 20:30 by Laserson

using Lidemia.Core.Enums;

namespace Lidemia.Core.Models.Misc;

/// <summary>
/// The payload that is stored in the "text" column of a chat message. The column is a JSON object so
/// every message type keeps only the data it needs, for example a plain message stores its text, a
/// product message stores the identifier of the shared product and an order message stores the
/// identifier of the shared order.
/// </summary>
public record ChatMessageContent
{
    public ChatMessageType Type { get; init; } = ChatMessageType.Message;

    public string? Text { get; init; }

    public long? ProductId { get; init; }

    public long? OrderId { get; init; }

    public static ChatMessageContent CreateText(string text) => new()
    {
        Type = ChatMessageType.Message,
        Text = text
    };

    public static ChatMessageContent CreateProduct(long productId) => new()
    {
        Type = ChatMessageType.Product,
        ProductId = productId
    };

    public static ChatMessageContent CreateOrder(long orderId) => new()
    {
        Type = ChatMessageType.Order,
        OrderId = orderId
    };
}
