// Created on 06/03/2021 12:07 by Andrey Laserson

using Lidemia.Core.Models.Misc;
using X.PagedList;
using X.PagedList.Extensions;

namespace Lidemia.Core.Extensions;

public static class PagedListExtensions
{
    public static Task<IPagedList<T>> ToPagedListEx<T>(this IQueryable<T> queryable, PagingInfoModel? pagingInfo, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        pagingInfo ??= new PagingInfoModel();
        return Task.FromResult(queryable.ToPagedList(pagingInfo.Page, pagingInfo.PageSize));
    }
}