// Created on 01/09/2026 14:59 by Laserson

using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lidemia.DataAccess.EntityConfigurations;

public class CustomerEntityConfiguration : IEntityTypeConfiguration<CustomerEntity>
{
    public void Configure(EntityTypeBuilder<CustomerEntity> builder)
    {
        builder.HasIndex(x => x.UserId).IsUnique();
        builder.Property(x => x.Metadata).AsJsonb();
        builder.AddBaseColumns<CustomerEntity, long>();
    }
}