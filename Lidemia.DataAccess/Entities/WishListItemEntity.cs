// Created on 16/09/2026 18:37 by Laserson

using Lidemia.Core.Models.Base;
using Lidemia.DataAccess.Abstract;

namespace Lidemia.DataAccess.Entities;

public class WishListItemEntity : IHasId<Guid>, IDbEntity
{
    public Guid Id { get; set; }

    public Guid WishListId { get; set; }

    public WishListEntity WishList { get; set; } = null!;

    public long ProductId { get; set; }

    public ProductEntity Product { get; set; } = null!;

    public int RowVersion { get; set; }

    public bool IsActive { get; set; }

    public DateTimeOffset CreateDate { get; set; }

    public DateTimeOffset? UpdateDate { get; set; }
}