// Created on 23/11/2026 19:32 by Laserson

using Lidemia.Core.Enums;
using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Repository.Abstract.Base;

namespace Lidemia.DataAccess.Repository.Abstract;

public interface IChatRepository : IRepository<ChatEntity, long>
{
    Task<ChatEntity?> Find(long customerId, long supplierId, CancellationToken cancellationToken);

    Task<ChatEntity?> GetOrCreate(long customerId, long supplierId, CancellationToken cancellationToken);

    Task<IReadOnlyList<ChatEntity>> GetCustomerChats(long customerId, CancellationToken cancellationToken);

    Task<IReadOnlyList<ChatEntity>> GetSupplierChats(long supplierId, CancellationToken cancellationToken);

    Task<ChatEntity?> GetChat(long chatId, long entityId, EntityType entityType, CancellationToken cancellationToken);

    Task<bool> HasAccess(long chatId, long entityId, EntityType entityType, CancellationToken cancellationToken);
}