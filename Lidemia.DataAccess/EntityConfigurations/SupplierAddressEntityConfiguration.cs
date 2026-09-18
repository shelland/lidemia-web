// Created on 18/09/2026 19:15 by Laserson

using Lidemia.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lidemia.DataAccess.EntityConfigurations;

public class SupplierAddressEntityConfiguration : IEntityTypeConfiguration<SupplierAddressEntity>
{
    public void Configure(EntityTypeBuilder<SupplierAddressEntity> builder)
    {
        builder.HasKey(x => new
        {
            x.SupplierId,
            x.AddressId
        });

        builder.HasIndex(x => new
        {
            x.SupplierId,
            x.IsActive
        });
    }
}