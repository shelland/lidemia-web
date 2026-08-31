// Created on 20/11/2021 12:43 by Andrey Laserson

using Lidemia.DataAccess.Abstract;
using Lidemia.DataAccess.Entities.Meta;

namespace Lidemia.DataAccess.Entities;

public class SupplierEntity : IDbEntity, IHasId<long>, IHasMetadata<SupplierEntityMetadata>
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public UserEntity User { get; set; } = null!;

    public string FullName { get; set; } = string.Empty;

    public string ShortName { get; set; } = string.Empty;

    public string? ShortDescription { get; set; }

    public string? Description { get; set; }

    public string? LogoUrl { get; set; }

    public string? CoverUrl { get; set; }

    public string? Tin { get; set; }

    public bool IsVerified { get; set; }

    public string? LegalAddress { get; set; }

    public string? ActualAddress { get; set; }

    public string? ContactPersonName { get; set; }

    public string? ContactPersonRole { get; set; }

    public string? Phone { get; set; }

    public string? CountryId { get; set; }

    // public CountryEntity? Country { get; set; }

    public bool IsEmailConfirmed { get; set; }

    public SupplierEntityMetadata Metadata { get; set; } = null!;

    public int RowVersion { get; set; }

    public bool IsActive { get; set; }

    public DateTimeOffset CreateDate { get; set; }

    public DateTimeOffset? UpdateDate { get; set; }
}