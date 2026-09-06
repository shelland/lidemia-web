// Created on 01/09/2026 15:00 by Laserson

using Lidemia.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lidemia.DataAccess.EntityConfigurations;

public class PhotoEntityConfiguration : IEntityTypeConfiguration<PhotoEntity>
{
    public void Configure(EntityTypeBuilder<PhotoEntity> builder)
    {
        builder.ToTable("photos");

        builder.Property(x => x.LargeThumbUrl).HasMaxLength(256);
        builder.Property(x => x.MediumThumbUrl).HasMaxLength(256);
        builder.Property(x => x.SmallThumbUrl).HasMaxLength(256);
        builder.Property(x => x.ExtraPath).HasMaxLength(256);
    }
}