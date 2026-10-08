// Created on 02/10/2026 20:34 by Laserson

using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lidemia.DataAccess.EntityConfigurations;

public class RegionEntityConfiguration : IEntityTypeConfiguration<RegionEntity>
{
    public void Configure(EntityTypeBuilder<RegionEntity> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(250);
        builder.HasIndex(x => x.CountryId);

        builder.AddBaseColumns<RegionEntity, Guid>();
    }
}