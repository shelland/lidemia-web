// Created on 23/11/2026 19:31 by Laserson

using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lidemia.DataAccess.EntityConfigurations;

public class ChatEntityConfiguration : IEntityTypeConfiguration<ChatEntity>
{
    public void Configure(EntityTypeBuilder<ChatEntity> builder)
    {
        builder.Property(x => x.LastMessageText).HasMaxLength(2000);

        builder.HasOne(x => x.Customer).WithMany().HasForeignKey(x => x.CustomerId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Supplier).WithMany().HasForeignKey(x => x.SupplierId).OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Messages)
            .WithOne(x => x.Chat)
            .HasForeignKey(x => x.ChatId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.CustomerId, x.SupplierId }).IsUnique()
            .HasFilter("\"is_active\"");

        builder.HasIndex(x => new { x.CustomerId, x.IsActive });
        builder.HasIndex(x => new { x.SupplierId, x.IsActive });

        builder.AddBaseColumns<ChatEntity, long>();
    }
}