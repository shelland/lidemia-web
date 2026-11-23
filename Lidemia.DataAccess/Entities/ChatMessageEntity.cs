// Created on 23/11/2026 19:30 by Laserson

using Lidemia.Core.Enums;
using Lidemia.Core.Models.Base;
using Lidemia.Core.Models.Misc;
using Lidemia.DataAccess.Abstract;

namespace Lidemia.DataAccess.Entities;

public class ChatMessageEntity : IDbEntity, IHasId<long>
{
    public long Id { get; set; }

    public long ChatId { get; set; }

    public ChatEntity Chat { get; set; } = null!;

    public EntityType SenderType { get; set; }

    public long SenderId { get; set; }

    public ChatMessageContent Text { get; set; } = new();

    public bool IsRead { get; set; }

    public DateTimeOffset? ReadDate { get; set; }

    public int RowVersion { get; set; }

    public bool IsActive { get; set; }

    public DateTimeOffset CreateDate { get; set; }

    public DateTimeOffset? UpdateDate { get; set; }
}