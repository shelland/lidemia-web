// Created on 01/09/2026 15:04 by Laserson

using Lidemia.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lidemia.DataAccess.EntityConfigurations;

public class PromoCodeEntityConfiguration : IEntityTypeConfiguration<PromoCodeEntity>
{
    public void Configure(EntityTypeBuilder<PromoCodeEntity> builder)
    {
        builder.ToTable("promo_codes");
    }
}