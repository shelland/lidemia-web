// Created on 02/09/2026 20:18 by Laserson

using Lidemia.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lidemia.DataAccess.EntityConfigurations;

public class ProductPhotoEntityConfiguration : IEntityTypeConfiguration<ProductPhotoEntity>
{
    public void Configure(EntityTypeBuilder<ProductPhotoEntity> builder)
    {
        throw new NotImplementedException();
    }
}