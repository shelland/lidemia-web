// Created on 01/09/2026 15:04 by Laserson

using Lidemia.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lidemia.DataAccess.EntityConfigurations;

public class OrderEntityConfiguration : IEntityTypeConfiguration<OrderEntity>
{
    public void Configure(EntityTypeBuilder<OrderEntity> builder)
    {
        builder.ToTable("orders");

        builder.Property(x => x.Number).HasMaxLength(32);

        builder.HasIndex(x => new { x.CustomerId, x.IsActive });
    }
}