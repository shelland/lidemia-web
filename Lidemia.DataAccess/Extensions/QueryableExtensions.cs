// Created on 27/10/2022 22:33 by shell

using Lidemia.DataAccess.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Lidemia.DataAccess.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> AsActive<T>(this IQueryable<T> db) where T : class, IDbEntity
    {
        return db.Where(x => x.IsActive);
    }

    public static Task<int> SoftDelete<TEntity, TKey>(this IQueryable<TEntity> query, TKey key, CancellationToken cancellationToken = default)
        where TEntity : class, IDbEntity, IHasId<TKey>
        where TKey : notnull
    {
        return query.AsActive()
            .Where(x => x.Id.Equals(key))
            .ExecuteUpdateAsync(x => x.SetProperty(e => e.IsActive, false), cancellationToken: cancellationToken);
    }
}