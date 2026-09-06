// Created on 02/12/2021 21:23 by Andrey Laserson

using Lidemia.Core.Models.Base;
using Lidemia.DataAccess.Abstract;

namespace Lidemia.DataAccess.Entities;

public class PhotoEntity : IDbEntity, IHasId<long>
{
    public long Id { get; set; }

    public string? LargeThumbUrl { get; set; }

    public string? MediumThumbUrl { get; set; }

    public string? SmallThumbUrl { get; set; }

    public string? ExtraPath { get; set; }

    public bool IsProcessed { get; set; }

    public long UserId { get; set; }

    public UserEntity User { get; set; } = null!;

    public int RowVersion { get; set; }

    public bool IsActive { get; set; }

    public DateTimeOffset CreateDate { get; set; }

    public DateTimeOffset? UpdateDate { get; set; }
}