// Created on 23/11/2026 19:32 by Laserson

using Lidemia.Core.Enums;
using Lidemia.Core.Models.Misc;
using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Repository.Abstract.Base;

namespace Lidemia.DataAccess.Repository.Abstract;

public interface IChatMessageRepository : IRepository<ChatMessageEntity, long>
{
    Task<ChatMessageEntity> Add(long chatId, EntityType senderType, long senderId, ChatMessageContent content, string? lastMessageText,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<ChatMessageEntity>> GetMessages(long chatId, int limit, CancellationToken cancellationToken);

    Task<Dictionary<long, int>> GetUnreadCounts(IReadOnlyCollection<long> chatIds, EntityType readerType, CancellationToken cancellationToken);

    Task<int> MarkRead(long chatId, EntityType readerType, CancellationToken cancellationToken);
}
