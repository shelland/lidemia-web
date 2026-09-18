// Created on 02/09/2026 20:16 by Laserson

using Lidemia.Core.Models.Base;
using Lidemia.DataAccess.Abstract;

namespace Lidemia.DataAccess.Entities;

public class WishListEntity : IHasId<Guid>, IDbEntity
{
    public Guid Id { get; set; }

    public CustomerEntity Customer { get; set; } = null!;

    public long CustomerId { get; set; }

    public string? Name { get; set; }

    public int ItemsCount { get; set; }

    public int RowVersion { get; set; }

    public bool IsActive { get; set; }

    public DateTimeOffset CreateDate { get; set; }

    public DateTimeOffset? UpdateDate { get; set; }
}