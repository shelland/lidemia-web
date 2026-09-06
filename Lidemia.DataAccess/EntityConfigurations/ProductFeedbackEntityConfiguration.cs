// Created on 04/09/2026 18:52 by Laserson

using Lidemia.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lidemia.DataAccess.EntityConfigurations;

public class ProductFeedbackEntityConfiguration : IEntityTypeConfiguration<ProductFeedbackEntity>
{
    public void Configure(EntityTypeBuilder<ProductFeedbackEntity> builder)
    {
        builder.ToTable("product_feedback");

        builder.Property(x => x.Comment).HasMaxLength(5000);

        builder.HasIndex(x => new { x.ProductId, x.IsActive });
        builder.HasIndex(x => new { x.CustomerId, x.IsActive });
    }
}