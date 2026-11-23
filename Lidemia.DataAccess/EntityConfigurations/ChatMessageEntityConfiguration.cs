// Created on 23/11/2026 19:31 by Laserson

using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lidemia.DataAccess.EntityConfigurations;

public class ChatMessageEntityConfiguration : IEntityTypeConfiguration<ChatMessageEntity>
{
    public void Configure(EntityTypeBuilder<ChatMessageEntity> builder)
    {
        builder.Property(x => x.Text).AsJsonb();

        builder.HasOne(x => x.Chat)
            .WithMany(x => x.Messages)
            .HasForeignKey(x => x.ChatId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.ChatId, x.IsActive });
        builder.HasIndex(x => new { x.ChatId, x.IsRead });
        builder.HasIndex(x => new { x.ChatId, x.CreateDate });

        builder.AddBaseColumns<ChatMessageEntity, long>();
    }
}