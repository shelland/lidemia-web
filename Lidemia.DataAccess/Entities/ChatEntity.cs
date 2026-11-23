// Created on 23/11/2026 19:30 by Laserson

using Lidemia.Core.Models.Base;
using Lidemia.DataAccess.Abstract;

namespace Lidemia.DataAccess.Entities;

public class ChatEntity : IDbEntity, IHasId<long>
{
    public long Id { get; set; }

    public long CustomerId { get; set; }

    public CustomerEntity Customer { get; set; } = null!;

    public long SupplierId { get; set; }

    public SupplierEntity Supplier { get; set; } = null!;

    public string? LastMessageText { get; set; }

    public DateTimeOffset? LastMessageDate { get; set; }

    public ICollection<ChatMessageEntity> Messages { get; set; } = null!;

    public int RowVersion { get; set; }

    public bool IsActive { get; set; }

    public DateTimeOffset CreateDate { get; set; }

    public DateTimeOffset? UpdateDate { get; set; }
}