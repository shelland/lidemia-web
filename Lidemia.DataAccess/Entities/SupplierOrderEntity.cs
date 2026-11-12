// Created on 04/11/2026 13:57 by Laserson

using Lidemia.Core.Enums;
using Lidemia.Core.Models.Base;
using Lidemia.Core.Models.Misc;
using Lidemia.DataAccess.Abstract;

namespace Lidemia.DataAccess.Entities;

public class SupplierOrderEntity : IDbEntity, IHasId<long>
{
    public long Id { get; set; }

    public long SupplierId { get; set; }

    public SupplierEntity Supplier { get; set; } = null!;

    public SupplierOrderItems Items { get; set; } = null!;

    public long OrderId { get; set; }

    public OrderEntity Order { get; set; } = null!;

    public OrderStatus Status { get; set; }

    public SupplierAddressEntity? SupplierAddress { get; set; }

    public long? SupplierAddressId { get; set; }

    public CustomerAddressEntity? CustomerAddress { get; set; }

    public long? CustomerAddressId { get; set; }

    public DateTimeOffset? LastStatusUpdate { get; set; }

    public long? PromoCodeId { get; set; }

    public PromoCodeEntity? PromoCode { get; set; }

    public DeliveryType DeliveryType { get; set; }

    public int RowVersion { get; set; }

    public bool IsActive { get; set; }

    public DateTimeOffset CreateDate { get; set; }

    public DateTimeOffset? UpdateDate { get; set; }
}