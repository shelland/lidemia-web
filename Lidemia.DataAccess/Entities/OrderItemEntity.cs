// Created on 04/09/2026 18:56 by Laserson

using Lidemia.DataAccess.Abstract;

namespace Lidemia.DataAccess.Entities;

public class OrderItemEntity : IDbEntity
{
    public long OrderId { get; set; }

    public OrderEntity Order { get; set; } = null!;

    public long ProductId { get; set; }

    public ProductEntity Product { get; set; } = null!;

    public double Quantity { get; set; }

    public int RowVersion { get; set; }

    public bool IsActive { get; set; }

    public DateTimeOffset CreateDate { get; set; }

    public DateTimeOffset? UpdateDate { get; set; }
}