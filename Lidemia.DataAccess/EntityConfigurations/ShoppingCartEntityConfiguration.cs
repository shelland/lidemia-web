// Created on 02/09/2026 20:20 by Laserson

using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lidemia.DataAccess.EntityConfigurations;

public class ShoppingCartEntityConfiguration : IEntityTypeConfiguration<ShoppingCartEntity>
{
    public void Configure(EntityTypeBuilder<ShoppingCartEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Items).AsJsonb();

        builder.HasIndex(x => new { x.UserId, x.IsActive });
    }
}