// Created on 16/09/2026 18:38 by Laserson

using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lidemia.DataAccess.EntityConfigurations;

public class WishListItemEntityConfiguration : IEntityTypeConfiguration<WishListItemEntity>
{
    public void Configure(EntityTypeBuilder<WishListItemEntity> builder)
    {
        builder.Property(x => x.Id).AsGuidV7();

        builder.HasIndex(x => new
        {
            x.WishListId,
            x.IsActive
        });

        builder.HasIndex(x => new
        {
            x.WishListId,
            x.ProductId
        }).IsUnique();
    }
}