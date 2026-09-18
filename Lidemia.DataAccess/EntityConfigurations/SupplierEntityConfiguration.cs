// Created on 01/09/2026 15:00 by Laserson

using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lidemia.DataAccess.EntityConfigurations;

public class SupplierEntityConfiguration : IEntityTypeConfiguration<SupplierEntity>
{
    public void Configure(EntityTypeBuilder<SupplierEntity> builder)
    {
        builder.HasIndex(x => x.UserId).IsUnique();

        builder.Property(x => x.Metadata).AsJsonb();
        builder.Property(x => x.Name).HasMaxLength(250);
        builder.Property(x => x.Inn).HasMaxLength(12);
        builder.Property(x => x.Ogrn).HasMaxLength(15);
        builder.Property(x => x.CoverUrl).HasMaxLength(250);
        builder.Property(x => x.LogoUrl).HasMaxLength(250);

        builder.AddBaseColumns<SupplierEntity, long>();
    }
}