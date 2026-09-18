// Created on 18/09/2026 19:14 by Laserson

using Lidemia.DataAccess.Abstract;

namespace Lidemia.DataAccess.Entities;

public class CustomerAddressEntity : IDbEntity
{
    public long CustomerId { get; set; }

    public CustomerEntity Customer { get; set; } = null!;

    public long AddressId { get; set; }

    public AddressEntity Address { get; set; } = null!;

    public int RowVersion { get; set; }

    public bool IsActive { get; set; }

    public DateTimeOffset CreateDate { get; set; }

    public DateTimeOffset? UpdateDate { get; set; }
}