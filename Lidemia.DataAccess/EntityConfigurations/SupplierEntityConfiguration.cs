// Created on 01/09/2026 15:00 by Laserson

using Lidemia.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lidemia.DataAccess.EntityConfigurations;

public class SupplierEntityConfiguration : IEntityTypeConfiguration<SupplierEntity>
{
    public void Configure(EntityTypeBuilder<SupplierEntity> builder)
    {
        builder.ToTable("suppliers");
        builder.HasIndex(x => x.UserId).IsUnique();
    }
}