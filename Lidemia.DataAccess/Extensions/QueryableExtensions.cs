// Created on 27/10/2022 22:33 by shell

using Lidemia.Core.Extensions;
using Lidemia.Core.Models.Base;
using Lidemia.Core.Models.Misc;
using Lidemia.DataAccess.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
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
}