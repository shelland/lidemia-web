// Created on 14/11/2026 16:03 by Laserson

using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lidemia.DataAccess.EntityConfigurations;

public class ProductReviewStatsEntityConfiguration : IEntityTypeConfiguration<ProductReviewStatsEntity>
{
    public void Configure(EntityTypeBuilder<ProductReviewStatsEntity> builder)
    {
        builder.HasKey(x => x.ProductId);
        builder.Property(x => x.Distribution).AsJsonb();

        builder.Property(e => e.RowVersion).HasDefaultValue(1);

        builder.Property(e => e.IsActive).IsRequired();

        builder.Property(e => e.CreateDate).HasDefaultValueSql("now()");
        builder.Property(e => e.UpdateDate);

        builder.HasIndex(x => x.IsActive);
    }
}