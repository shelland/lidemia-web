// Created on 02/09/2026 20:18 by Laserson

using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lidemia.DataAccess.EntityConfigurations;

public class WishListEntityConfiguration : IEntityTypeConfiguration<WishListEntity>
{
    public void Configure(EntityTypeBuilder<WishListEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Items).AsJsonb();

        builder.HasIndex(x => new { x.UserId, x.IsActive });
    }
}