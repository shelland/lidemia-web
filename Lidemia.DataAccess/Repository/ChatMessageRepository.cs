// Created on 23/11/2026 19:34 by Laserson

using Lidemia.Core.Enums;
using Lidemia.Core.Models.Misc;
using Lidemia.DataAccess.Context;
using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Extensions;
using Lidemia.DataAccess.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.DataAccess.Repository;

[ServiceDescriptor<IChatMessageRepository>(ServiceLifetime.Scoped)]
public class ChatMessageRepository : IChatMessageRepository
{
    private readonly LidemiaDbContext context;

    public ChatMessageRepository(LidemiaDbContext context)
    {
        this.context = context;
    }

    public Task<ChatMessageEntity?> GetById(long key, CancellationToken cancellationToken)
    {
        return this.context.ChatMessages
            .AsActive()
            .FirstOrDefaultAsync(x => x.Id == key, cancellationToken);
    }

    public Task Delete(long key, CancellationToken cancellationToken)
    {
        return this.context.ChatMessages
            .AsActive()
            .Where(x => x.Id == key)
            .ExecuteUpdateAsync(x => x.SetProperty(e => e.IsActive, false), cancellationToken);
    }

    public async Task<ChatMessageEntity> Add(long chatId, EntityType senderType, long senderId, ChatMessageContent content,
        string? lastMessageText, CancellationToken cancellationToken)
    {
        var entity = new ChatMessageEntity
        {
            ChatId = chatId,
            SenderType = senderType,
            SenderId = senderId,
            Text = content
        };

        this.context.ChatMessages.Add(entity);
        await this.context.SaveChangesAsync(cancellationToken);

        var chat = await this.context.Chats
            .AsTracking()
            .FirstOrDefaultAsync(x => x.Id == chatId, cancellationToken);

        if (chat != null)
        {
            chat.LastMessageText = lastMessageText;
            chat.LastMessageDate = entity.CreateDate;

            await this.context.SaveChangesAsync(cancellationToken);
        }

        return entity;
    }

    public async Task<IReadOnlyList<ChatMessageEntity>> GetMessages(long chatId, int limit, CancellationToken cancellationToken)
    {
        var messages = await this.context.ChatMessages
            .AsActive()
            .Where(x => x.ChatId == chatId)
            .OrderByDescending(x => x.Id)
            .Take(limit)
            .ToListAsync(cancellationToken);

        messages.Reverse();

        return messages;
    }

    public async Task<Dictionary<long, int>> GetUnreadCounts(IReadOnlyCollection<long> chatIds, EntityType readerType, CancellationToken cancellationToken)
    {
        if (chatIds.Count == 0)
        {
            return new Dictionary<long, int>();
        }

        return await this.context.ChatMessages
            .AsActive()
            .Where(x => chatIds.Contains(x.ChatId) && !x.IsRead && x.SenderType != readerType)
            .GroupBy(x => x.ChatId)
            .Select(x => new
            {
                x.Key,
                Count = x.Count()
            })
            .ToDictionaryAsync(x => x.Key, x => x.Count, cancellationToken);
    }

    public Task<int> MarkRead(long chatId, EntityType readerType, CancellationToken cancellationToken)
    {
        var readDate = DateTimeOffset.UtcNow;

        return this.context.ChatMessages
            .AsActive()
            .Where(x => x.ChatId == chatId && x.SenderType != readerType && !x.IsRead)
            .ExecuteUpdateAsync(x => x
                .SetProperty(m => m.IsRead, true)
                .SetProperty(m => m.ReadDate, readDate), cancellationToken);
    }
}