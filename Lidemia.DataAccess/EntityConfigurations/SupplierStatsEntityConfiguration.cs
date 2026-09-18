// Created on 17/09/2026 21:53 by Laserson

using Lidemia.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lidemia.DataAccess.EntityConfigurations;

public class SupplierStatsEntityConfiguration : IEntityTypeConfiguration<SupplierStatsEntity>
{
    public void Configure(EntityTypeBuilder<SupplierStatsEntity> builder)
    {
        builder.HasNoKey();
    }
}