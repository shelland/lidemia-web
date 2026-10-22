// Created on 02/09/2026 20:17 by Laserson

using Lidemia.Core.Models.Base;
using Lidemia.DataAccess.Abstract;
using Lidemia.DataAccess.Entities.Misc;

namespace Lidemia.DataAccess.Entities;

public class ShoppingCartEntity : IDbEntity, IHasId<long>
{
    public long Id { get; set; }

    public CustomerEntity Customer { get; set; } = null!;

    public long CustomerId { get; set; }

    public IEnumerable<ShoppingCartItemDbModel> Items { get; set; } = [];

    public int RowVersion { get; set; }

    public bool IsActive { get; set; }

    public DateTimeOffset CreateDate { get; set; }

    public DateTimeOffset? UpdateDate { get; set; }
}