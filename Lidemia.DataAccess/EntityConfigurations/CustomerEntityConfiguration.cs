// Created on 01/09/2026 14:59 by Laserson

using Lidemia.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lidemia.DataAccess.EntityConfigurations;

public class CustomerEntityConfiguration : IEntityTypeConfiguration<CustomerEntity>
{
    public void Configure(EntityTypeBuilder<CustomerEntity> builder)
    {
        builder.ToTable("customers");

        builder.HasIndex(x => x.UserId).IsUnique();
    }
}