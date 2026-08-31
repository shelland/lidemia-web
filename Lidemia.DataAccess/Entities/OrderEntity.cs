// Created on 20/11/2021 12:43 by Andrey Laserson

using Lidemia.Core.Enums;
using Lidemia.DataAccess.Abstract;
using Lidemia.DataAccess.Entities.Meta;
using Lidemia.DataAccess.Entities.Misc;

namespace Lidemia.DataAccess.Entities;

public class OrderEntity : IDbEntity, IHasId<long>, IHasMetadata<OrderMetadata>
{
    public long Id { get; set; }

    public string Number { get; set; } = string.Empty;

    public long SupplierId { get; set; }

    public SupplierEntity Supplier { get; set; } = null!;

    public long CustomerId { get; set; }

    public CustomerEntity Customer { get; set; } = null!;

    public OrderStatus Status { get; set; }

    public IReadOnlyCollection<OrderItemDbModel> Items { get; set; } = [];

    public string? Comment { get; set; }

    public bool IsFinishConfirmed { get; set; }

    public DateTime? LastStatusUpdate { get; set; }

    public OrderMetadata Metadata { get; set; } = null!;

    public long? PromoCodeId { get; set; }

    public PromoCodeEntity? PromoCode { get; set; }

    public int RowVersion { get; set; }

    public bool IsActive { get; set; }

    public DateTimeOffset CreateDate { get; set; }

    public DateTimeOffset? UpdateDate { get; set; }
}