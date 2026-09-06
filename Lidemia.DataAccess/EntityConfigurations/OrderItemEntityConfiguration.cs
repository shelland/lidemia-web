// Created on 04/09/2026 18:57 by Laserson

using Lidemia.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lidemia.DataAccess.EntityConfigurations;

public class OrderItemEntityConfiguration : IEntityTypeConfiguration<OrderItemEntity>
{
    public void Configure(EntityTypeBuilder<OrderItemEntity> builder)
    {
        builder.ToTable("order_items");

        builder.HasIndex(x => new { x.OrderId, x.IsActive });
        builder.HasIndex(x => new { x.ProductId, x.IsActive });
    }
}