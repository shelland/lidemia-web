// Created on 17/09/2026 21:53 by Laserson

using Lidemia.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lidemia.DataAccess.EntityConfigurations;

public class SupplierStatsEntityConfiguration : IEntityTypeConfiguration<SupplierStatsEntity>
{
    public void Configure(EntityTypeBuilder<SupplierStatsEntity> builder)
    {
        builder.HasKey(x => x.SupplierId);

        builder.HasOne(x => x.Supplier)
            .WithOne(x => x.Stats)
            .HasForeignKey<SupplierStatsEntity>(e => e.SupplierId);

        builder.Property(e => e.RowVersion).HasDefaultValue(1);

        builder.Property(e => e.IsActive).IsRequired();

        builder.Property(e => e.CreateDate).HasDefaultValueSql("now()");
        builder.Property(e => e.UpdateDate);

        builder.HasIndex(x => x.IsActive);
    }
}