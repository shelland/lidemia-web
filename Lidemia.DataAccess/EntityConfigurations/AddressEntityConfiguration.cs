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
        builder.Property(x => x.Id).AsGuidV7();

        builder.Property(x => x.PostalIndex).HasMaxLength(6);
        builder.Property(x => x.AddressLine1).HasMaxLength(250);
        builder.Property(x => x.AddressLine2).HasMaxLength(250);
        builder.Property(x => x.City).HasMaxLength(250);
        builder.Property(x => x.Region).HasMaxLength(250);

        builder.HasIndex(x => new
        {
            x.CustomerId,
            x.IsActive
        });

        builder.HasIndex(x => new
        {
            x.CustomerId,
            x.IsDefault,
            x.IsActive
        });
    }
}