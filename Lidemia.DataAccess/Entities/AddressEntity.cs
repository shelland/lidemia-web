// Created on 17/09/2026 22:29 by Laserson

using Lidemia.Core.Models.Base;
using Lidemia.DataAccess.Abstract;

namespace Lidemia.DataAccess.Entities;

public class AddressEntity : IHasId<Guid>, IDbEntity
{
    public Guid Id { get; set; }

    public long CustomerId { get; set; }

    public CustomerEntity Customer { get; set; } = null!;

    public bool IsDefault { get; set; }

    public string PostalIndex { get; set; } = string.Empty;

    public string Region { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string AddressLine1 { get; set; } = string.Empty;

    public string? AddressLine2 { get; set; }

    public int RowVersion { get; set; }

    public bool IsActive { get; set; }

    public DateTimeOffset CreateDate { get; set; }

    public DateTimeOffset? UpdateDate { get; set; }
}