// Created on 23/11/2026 19:36 by Laserson

using Lidemia.Core.Enums;
using Lidemia.Core.Models.Base;

namespace Lidemia.Core.Models.Domain;

public class ChatMessageModel : AbstractEntity<long>
{
    public long ChatId { get; set; }

    public EntityType SenderType { get; set; }

    public long SenderId { get; set; }

    public ChatMessageType Type { get; set; }

    public string? Text { get; set; }

    public long? ProductId { get; set; }

    public long? OrderId { get; set; }

    public bool IsRead { get; set; }
}
