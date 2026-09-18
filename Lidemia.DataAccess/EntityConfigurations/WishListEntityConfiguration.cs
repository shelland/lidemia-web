// Created on 02/09/2026 20:18 by Laserson

using Lidemia.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lidemia.DataAccess.EntityConfigurations;

public class WishListEntityConfiguration : IEntityTypeConfiguration<WishListEntity>
{
    public void Configure(EntityTypeBuilder<WishListEntity> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(500);

        builder.HasIndex(x => new
        {
            UserId = x.CustomerId,
            x.IsActive
        });
    }
}