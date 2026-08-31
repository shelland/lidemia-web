// Created on 12/01/2020 13:16 by Andrey Laserson

using Lidemia.DataAccess.Abstract;

namespace Lidemia.DataAccess.Repository.Abstract.Base;

public interface IRepository<TModel, in TKey>
    where TModel : IDbEntity
    where TKey : notnull
{
    Task<TModel?> GetById(TKey key, CancellationToken cancellationToken);

    Task Delete(TKey key, CancellationToken cancellationToken);
}