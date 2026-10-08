// Created on 02/10/2026 20:35 by Laserson

using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lidemia.DataAccess.EntityConfigurations;

public class CityEntityConfiguration : IEntityTypeConfiguration<CityEntity>
{
    public void Configure(EntityTypeBuilder<CityEntity> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(250);
        builder.HasIndex(x => x.RegionId);

        builder.AddBaseColumns<CityEntity, Guid>();
    }
}