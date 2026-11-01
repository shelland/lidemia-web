// Created on 04/09/2026 18:52 by Laserson

using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lidemia.DataAccess.EntityConfigurations;

public class ProductReviewEntityConfiguration : IEntityTypeConfiguration<ProductReviewEntity>
{
    public void Configure(EntityTypeBuilder<ProductReviewEntity> builder)
    {
        builder.AddBaseColumns<ProductReviewEntity, long>();

        builder.Property(x => x.Review).HasMaxLength(5000);
        builder.Property(x => x.PositiveReview).HasMaxLength(5000);
        builder.Property(x => x.NegativeReview).HasMaxLength(5000);

        builder.HasIndex(x => new
        {
            x.ProductId,
            x.State,
            x.IsActive,
            x.CreateDate
        });

        builder.HasIndex(x => new
        {
            x.ProductId,
            x.IsActive
        });

        builder.HasIndex(x => new
        {
            x.CustomerId,
            x.IsActive
        });

        builder.HasIndex(x => new
        {
            x.ProductId,
            x.CustomerId,
            x.IsActive
        });
    }
}