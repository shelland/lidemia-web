// Created on 04/11/2026 14:40 by Laserson

using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lidemia.DataAccess.EntityConfigurations;

public class SupplierOrderEntityConfiguration : IEntityTypeConfiguration<SupplierOrderEntity>
{
    public void Configure(EntityTypeBuilder<SupplierOrderEntity> builder)
    {
        builder.AddBaseColumns<SupplierOrderEntity, long>();

        builder.HasIndex(x => new
        {
            x.SupplierId,
            x.IsActive
        });

        builder.HasIndex(x => new
        {
            x.Status,
            x.SupplierId,
            x.IsActive
        });
    }
}