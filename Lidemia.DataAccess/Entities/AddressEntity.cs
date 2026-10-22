// Created on 17/09/2026 22:29 by Laserson

using Lidemia.Core.Models.Base;
using Lidemia.DataAccess.Abstract;

namespace Lidemia.DataAccess.Entities;

public class AddressEntity : IHasId<long>, IDbEntity
{
    public long Id { get; set; }

    public bool IsDefault { get; set; }

    public string PostalIndex { get; set; } = string.Empty;

    public CountryEntity Country { get; set; } = null!;

    public long CountryId { get; set; }

    public RegionEntity Region { get; set; } = null!;

    public long RegionId { get; set; }

    public CityEntity City { get; set; } = null!;

    public long CityId { get; set; }

    public string StreetName { get; set; } = string.Empty;
    
    public string HouseNumber { get; set; } = string.Empty;

    public string? HouseAdd1 { get; set; }

    public string? HouseAdd2 { get; set; }

    public string? FlatNumber { get; set; }

    public string? AddressLine1 { get; set; }

    public string? AddressLine2 { get; set; }

    public int RowVersion { get; set; }

    public bool IsActive { get; set; }

    public DateTimeOffset CreateDate { get; set; }

    public DateTimeOffset? UpdateDate { get; set; }
}