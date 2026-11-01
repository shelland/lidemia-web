// Created on 29/10/2026 18:56 by Laserson

using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Repository.Abstract.Base;

namespace Lidemia.DataAccess.Repository.Abstract;

public interface IProductCategoryRepository : IRepository<ProductCategoryEntity, long>
{
    Task<IEnumerable<ProductCategoryEntity>> GetCategories(CancellationToken cancellationToken);
}