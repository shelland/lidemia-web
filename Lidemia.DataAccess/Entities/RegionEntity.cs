// Created on 02/10/2026 20:13 by Laserson

using Lidemia.Core.Models.Base;
using Lidemia.DataAccess.Abstract;

namespace Lidemia.DataAccess.Entities;

public class RegionEntity : IHasId<Guid>, IDbEntity
{
    public Guid Id { get; set; }

    public CountryEntity Country { get; set; } = null!;

    public Guid CountryId { get; set; }

    public string Name { get; set; } = string.Empty;

    public int RowVersion { get; set; }

    public bool IsActive { get; set; }

    public DateTimeOffset CreateDate { get; set; }

    public DateTimeOffset? UpdateDate { get; set; }
}