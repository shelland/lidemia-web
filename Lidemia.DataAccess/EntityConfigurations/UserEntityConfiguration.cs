// Created on 04/09/2026 19:43 by Laserson

using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lidemia.DataAccess.EntityConfigurations;

public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.Property(x => x.Password).HasMaxLength(256);
        builder.Property(x => x.PasswordSalt).HasMaxLength(256);

        builder.Property(x => x.Email).HasMaxLength(256);
        builder.Property(x => x.EmailNormalized).HasMaxLength(256);

        builder.HasIndex(x => x.Email).IsUnique();
        builder.HasIndex(x => x.EmailNormalized).IsUnique();

        builder.Property(x => x.Phone).HasMaxLength(15);
        builder.HasIndex(x => x.Phone).IsUnique();

        builder.AddBaseColumns<UserEntity, long>();
    }
}