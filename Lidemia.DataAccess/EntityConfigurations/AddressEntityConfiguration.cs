// Created on 17/09/2026 22:34 by Laserson

using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lidemia.DataAccess.EntityConfigurations;

public class AddressEntityConfiguration : IEntityTypeConfiguration<AddressEntity>
{
    public void Configure(EntityTypeBuilder<AddressEntity> builder)
    {
        builder.Property(x => x.PostalIndex).HasMaxLength(6);
        builder.Property(x => x.AddressLine1).HasMaxLength(250);
        builder.Property(x => x.AddressLine2).HasMaxLength(250);
        builder.Property(x => x.HouseAdd1).HasMaxLength(50);
        builder.Property(x => x.HouseAdd2).HasMaxLength(50);
        builder.Property(x => x.StreetName).HasMaxLength(250);
        builder.Property(x => x.HouseNumber).HasMaxLength(50);
        builder.Property(x => x.FlatNumber).HasMaxLength(50);

        builder.AddBaseColumns<AddressEntity, Guid>();
    }
}