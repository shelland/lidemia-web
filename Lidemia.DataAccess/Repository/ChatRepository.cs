// Created on 23/11/2026 19:33 by Laserson

using System.Linq.Expressions;
using Lidemia.Core.Enums;
using Lidemia.DataAccess.Context;
using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Extensions;
using Lidemia.DataAccess.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.DataAccess.Repository;

[ServiceDescriptor<IChatRepository>(ServiceLifetime.Scoped)]
public class ChatRepository : IChatRepository
{
    private readonly LidemiaDbContext context;

    public ChatRepository(LidemiaDbContext context)
    {
        this.context = context;
    }

    public Task<ChatEntity?> GetById(long key, CancellationToken cancellationToken)
    {
        return this.context.Chats
            .AsActive()
            .Include(x => x.Customer)
            .Include(x => x.Supplier)
            .FirstOrDefaultAsync(x => x.Id == key, cancellationToken);
    }

    public Task Delete(long key, CancellationToken cancellationToken)
    {
        return this.context.Chats
            .AsActive()
            .Where(x => x.Id == key)
            .ExecuteUpdateAsync(x => x.SetProperty(e => e.IsActive, false), cancellationToken);
    }

    public Task<ChatEntity?> Find(long customerId, long supplierId, CancellationToken cancellationToken)
    {
        return this.context.Chats
            .AsActive()
            .Include(x => x.Customer)
            .Include(x => x.Supplier)
            .FirstOrDefaultAsync(x => x.CustomerId == customerId && x.SupplierId == supplierId, cancellationToken);
    }

    public async Task<ChatEntity?> GetOrCreate(long customerId, long supplierId, CancellationToken cancellationToken)
    {
        var existing = await this.Find(customerId, supplierId, cancellationToken);

        if (existing != null)
        {
            return existing;
        }

        var customerExists = await this.context.Customers.AsActive().AnyAsync(x => x.Id == customerId, cancellationToken);
        var supplierExists = await this.context.Suppliers.AsActive().AnyAsync(x => x.Id == supplierId, cancellationToken);

        if (!customerExists || !supplierExists)
        {
            return null;
        }

        var entity = new ChatEntity
        {
            CustomerId = customerId,
            SupplierId = supplierId
        };

        try
        {
            this.context.Chats.Add(entity);
            await this.context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            // Another request created the same chat in parallel, it is protected by the unique index.
            this.context.Entry(entity).State = EntityState.Detached;
            return await this.Find(customerId, supplierId, cancellationToken);
        }

        return await this.GetById(entity.Id, cancellationToken);
    }

    public async Task<IReadOnlyList<ChatEntity>> GetCustomerChats(long customerId, CancellationToken cancellationToken)
    {
        return await this.GetChatsQuery(x => x.CustomerId == customerId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ChatEntity>> GetSupplierChats(long supplierId, CancellationToken cancellationToken)
    {
        return await this.GetChatsQuery(x => x.SupplierId == supplierId)
            .ToListAsync(cancellationToken);
    }

    public Task<ChatEntity?> GetChat(long chatId, long entityId, EntityType entityType, CancellationToken cancellationToken)
    {
        var query = this.context.Chats
            .AsActive()
            .Include(x => x.Customer)
            .Include(x => x.Supplier)
            .Where(x => x.Id == chatId);

        return this.FilterByEntity(query, entityId, entityType)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<bool> HasAccess(long chatId, long entityId, EntityType entityType, CancellationToken cancellationToken)
    {
        var query = this.context.Chats
            .AsActive()
            .Where(x => x.Id == chatId);

        return this.FilterByEntity(query, entityId, entityType)
            .AnyAsync(cancellationToken);
    }

    private IQueryable<ChatEntity> GetChatsQuery(Expression<Func<ChatEntity, bool>> predicate)
    {
        return this.context.Chats
            .AsActive()
            .Include(x => x.Customer)
            .Include(x => x.Supplier)
            .Where(predicate)
            .OrderByDescending(x => x.LastMessageDate ?? x.CreateDate);
    }

    private IQueryable<ChatEntity> FilterByEntity(IQueryable<ChatEntity> query, long entityId, EntityType entityType)
    {
        return entityType switch
        {
            EntityType.Customer => query.Where(x => x.CustomerId == entityId),
            EntityType.Supplier => query.Where(x => x.SupplierId == entityId),
            _ => query.Where(x => false)
        };
    }
}