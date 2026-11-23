// Created on 23/11/2026 19:40 by Laserson

using FluentResults;
using Lidemia.Core.Enums;
using Lidemia.Core.Models.Domain;
using Lidemia.Core.Models.Misc;

namespace Lidemia.Common.BusinessLogic.Services.Data.Abstract;

public interface IChatService
{
    Task<Result<long>> GetOrCreateChat(long customerId, long supplierId, CancellationToken cancellationToken);

    Task<IReadOnlyList<ChatModel>> GetChats(long entityId, EntityType entityType, CancellationToken cancellationToken);

    Task<ChatModel?> GetChat(long chatId, long entityId, EntityType entityType, CancellationToken cancellationToken);

    Task<IReadOnlyList<ChatMessageModel>> GetMessages(long chatId, long entityId, EntityType entityType, CancellationToken cancellationToken);

    Task<Result<ChatMessageModel>> SendMessage(long chatId, long entityId, EntityType entityType, ChatMessageContent content,
        CancellationToken cancellationToken);

    Task MarkRead(long chatId, long entityId, EntityType entityType, CancellationToken cancellationToken);

    Task<bool> HasAccess(long chatId, long entityId, EntityType entityType, CancellationToken cancellationToken);
}