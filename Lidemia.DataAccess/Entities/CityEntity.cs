// Created on 02/10/2026 20:15 by Laserson

using Lidemia.Core.Models.Base;
using Lidemia.DataAccess.Abstract;

namespace Lidemia.DataAccess.Entities;

public class CityEntity : IHasId<Guid>, IDbEntity
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public RegionEntity Region { get; set; } = null!;

    public Guid RegionId { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public int RowVersion { get; set; }

    public bool IsActive { get; set; }

    public DateTimeOffset CreateDate { get; set; }

    public DateTimeOffset? UpdateDate { get; set; }
}