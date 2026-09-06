// Created on 27/12/2021 17:53 by shell

using Lidemia.Core.Enums;
using Lidemia.Core.Models.Base;
using Lidemia.DataAccess.Abstract;

namespace Lidemia.DataAccess.Entities;

public class PromoCodeEntity : IDbEntity, IHasId<long>
{
    public long Id { get; set; }

    public string Code { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime? StartsAt { get; set; }

    public DateTime? ExpiresAt { get; set; }

    public int? MaxUsages { get; set; }

    public decimal? MaxAllowedTotalSum { get; set; }

    public bool IsOneTime { get; set; }

    public int TotalUsages { get; set; }

    public long[] AllowedProducts { get; set; } = [];

    public long[] AllowedCustomers { get; set; } = [];

    public long[] AllowedCategories { get; set; } = [];

    public PromoCodeDiscountType DiscountType { get; set; }

    public decimal Value { get; set; }

    public long SupplierId { get; set; }

    public SupplierEntity Supplier { get; set; } = null!;

    public int RowVersion { get; set; }

    public bool IsActive { get; set; }

    public DateTimeOffset CreateDate { get; set; }

    public DateTimeOffset? UpdateDate { get; set; }
}