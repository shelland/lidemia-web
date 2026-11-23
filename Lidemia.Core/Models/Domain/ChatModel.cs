// Created on 23/11/2026 19:36 by Laserson

using Lidemia.Core.Models.Base;

namespace Lidemia.Core.Models.Domain;

public class ChatModel : AbstractEntity<long>
{
    public long CustomerId { get; set; }

    public long SupplierId { get; set; }

    public string CustomerName { get; set; } = string.Empty;

    public string SupplierName { get; set; } = string.Empty;

    public string? LastMessageText { get; set; }

    public DateTimeOffset? LastMessageDate { get; set; }

    public int UnreadCount { get; set; }
}