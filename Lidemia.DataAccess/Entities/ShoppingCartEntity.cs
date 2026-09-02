// Created on 02/09/2026 20:17 by Laserson

using Lidemia.DataAccess.Abstract;
using Lidemia.DataAccess.Entities.Misc;

namespace Lidemia.DataAccess.Entities;

public class ShoppingCartEntity : IDbEntity, IHasId<Guid>
{
    public Guid Id { get; set; }

    public UserEntity User { get; set; } = null!;

    public long UserId { get; set; }

    public IEnumerable<ShoppingCartItemDbModel> Items { get; set; } = [];

    public int RowVersion { get; set; }

    public bool IsActive { get; set; }

    public DateTimeOffset CreateDate { get; set; }

    public DateTimeOffset? UpdateDate { get; set; }
}