// Created on 27/10/2022 22:33 by shell

using Lidemia.Core.Extensions;
using Lidemia.Core.Models.Base;
using Lidemia.Core.Models.Misc;
using Lidemia.DataAccess.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using X.PagedList;

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

    /// <summary>
    /// Projects database entities to models using the explicitly defined field mapping.
    /// The projection is a part of the query and is translated to SQL, so exactly the requested columns are selected.
    /// </summary>
    public static IQueryable<TModel> ProjectTo<TEntity, TModel>(this IQueryable<TEntity> query,
        Expression<Func<TEntity, TModel>> projection)
        where TEntity : class, IDbEntity
    {
        return query.Select(projection);
    }

    public static Task<List<TModel>> ProjectToListAsync<TEntity, TModel>(this IQueryable<TEntity> query,
        Expression<Func<TEntity, TModel>> projection, CancellationToken cancellationToken = default)
        where TEntity : class, IDbEntity
    {
        return query.ProjectTo(projection).ToListAsync(cancellationToken);
    }

    public static Task<TModel?> ProjectToFirstOrDefaultAsync<TEntity, TModel>(this IQueryable<TEntity> query,
        Expression<Func<TEntity, TModel>> projection, CancellationToken cancellationToken = default)
        where TEntity : class, IDbEntity
    {
        return query.ProjectTo(projection).FirstOrDefaultAsync(cancellationToken);
    }

    public static Task<IPagedList<TModel>> ProjectToPagedListAsync<TEntity, TModel>(this IQueryable<TEntity> query,
        Expression<Func<TEntity, TModel>> projection, PagingInfoModel? pagingInfo = null,
        CancellationToken cancellationToken = default)
        where TEntity : class, IDbEntity
    {
        return query.ProjectTo(projection).ToPagedListEx(pagingInfo, cancellationToken);
    }

    public static async IAsyncEnumerable<T> StreamByKeysetAsync<T, TKey>(
        this IQueryable<T> source,
        Expression<Func<T, TKey>> keySelector,
        int batchSize = 1000,
        [EnumeratorCancellation] CancellationToken ct = default)
        where TKey : struct, IComparable<TKey>
    {
        var compiledKey = keySelector.Compile();
        TKey? lastKey = null;

        while (!ct.IsCancellationRequested)
        {
            var query = source;

            if (lastKey.HasValue)
            {
                // Builds: x => x.Id > lastKey.Value
                var param = keySelector.Parameters[0];
                var body = Expression.GreaterThan(keySelector.Body, Expression.Constant(lastKey.Value));
                var predicate = Expression.Lambda<Func<T, bool>>(body, param);
                query = query.Where(predicate);
            }

            var batch = await query
                .OrderBy(keySelector)
                .Take(batchSize)
                .ToListAsync(ct);

            if (batch.Count == 0) yield break;

            foreach (var item in batch)
            {
                yield return item;
            }

            lastKey = compiledKey(batch[^1]);

            if (batch.Count < batchSize) yield break;
        }
    }
}