// Created on 09/12/2021 23:43 by Andrey Laserson

using Lidemia.DataAccess.Abstract;

namespace Lidemia.DataAccess.Entities;

public class ProductPhotoEntity : IDbEntity
{
    public long PhotoId { get; set; }

    public PhotoEntity Photo { get; set; } = null!;

    public long ProductId { get; set; }

    public ProductEntity Product { get; set; } = null!;

    public bool IsPrimary { get; set; }

    public int RowVersion { get; set; }

    public bool IsActive { get; set; }

    public DateTimeOffset CreateDate { get; set; }

    public DateTimeOffset? UpdateDate { get; set; }
}