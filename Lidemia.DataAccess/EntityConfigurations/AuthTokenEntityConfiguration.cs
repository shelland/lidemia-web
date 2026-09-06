// Created on 11/12/2021 21:48 by Andrey Laserson

using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lidemia.DataAccess.EntityConfigurations;

public class AuthTokenEntityConfiguration : IEntityTypeConfiguration<AuthTokenEntity>
{
    public void Configure(EntityTypeBuilder<AuthTokenEntity> builder)
    {
        builder.Property(x => x.AccessToken).HasMaxLength(250);
        builder.Property(x => x.RefreshToken).HasMaxLength(250);

        builder.HasIndex(x => new
        {
            x.AccessToken,
            x.IsActive
        });

        builder.HasIndex(x => new
        {
            x.EntityId,
            x.IsActive
        });

        builder.AddBaseColumns();
    }
}