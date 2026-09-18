// Created on 01/09/2026 13:30 by Laserson

using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lidemia.DataAccess.EntityConfigurations;

public class ProductEntityConfiguration : IEntityTypeConfiguration<ProductEntity>
{
    public void Configure(EntityTypeBuilder<ProductEntity> builder)
    {
        builder.HasIndex(x => x.IsVisible);
        builder.AddBaseColumns<ProductEntity, long>();

        builder.Property(x => x.Sku).HasMaxLength(50);
        builder.Property(x => x.Title).HasMaxLength(250);
        
        builder.HasOne(x => x.Parent);
    }
}