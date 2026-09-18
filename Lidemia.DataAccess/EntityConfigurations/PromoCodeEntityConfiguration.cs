// Created on 01/09/2026 15:04 by Laserson

using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lidemia.DataAccess.EntityConfigurations;

public class PromoCodeEntityConfiguration : IEntityTypeConfiguration<PromoCodeEntity>
{
    public void Configure(EntityTypeBuilder<PromoCodeEntity> builder)
    {
        builder.Property(x => x.Code).HasMaxLength(250);
        builder.AddBaseColumns<PromoCodeEntity, long>();
    }
}