// Created on 02/09/2026 20:19 by Laserson

using Lidemia.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lidemia.DataAccess.EntityConfigurations;

public class PasswordResetEntityConfiguration : IEntityTypeConfiguration<PasswordResetEntity>
{
    public void Configure(EntityTypeBuilder<PasswordResetEntity> builder)
    {
        throw new NotImplementedException();
    }
}