// Created on 18/09/2026 19:14 by Laserson

using Lidemia.Core.Enums;
using Lidemia.Core.Models.Base;
using Lidemia.DataAccess.Abstract;

namespace Lidemia.DataAccess.Entities;

public class SupplierAddressEntity : IDbEntity, IHasId<Guid>
{
    public Guid Id { get; set; }

    public long SupplierId { get; set; }

    public SupplierEntity Supplier { get; set; } = null!;

    public long AddressId { get; set; }

    public AddressEntity Address { get; set; } = null!;

    public SupplierAddressType Type { get; set; }

    public int RowVersion { get; set; }

    public bool IsActive { get; set; }

    public DateTimeOffset CreateDate { get; set; }

    public DateTimeOffset? UpdateDate { get; set; }
}