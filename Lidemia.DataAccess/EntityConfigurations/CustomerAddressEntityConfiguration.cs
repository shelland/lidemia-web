// Created on 18/09/2026 19:15 by Laserson

using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lidemia.DataAccess.EntityConfigurations;

public class CustomerAddressEntityConfiguration : IEntityTypeConfiguration<CustomerAddressEntity>
{
    public void Configure(EntityTypeBuilder<CustomerAddressEntity> builder)
    {
        builder.AddBaseColumns<CustomerAddressEntity, Guid>();

        builder.HasIndex(x => new
        {
            x.CustomerId,
            x.IsActive
        });
    }
}