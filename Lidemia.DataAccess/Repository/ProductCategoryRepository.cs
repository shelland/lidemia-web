// Created on 29/10/2026 18:56 by Laserson

using Lidemia.DataAccess.Context;
using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Extensions;
using Lidemia.DataAccess.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.DataAccess.Repository;

[ServiceDescriptor<IProductCategoryRepository>(ServiceLifetime.Scoped)]
public class ProductCategoryRepository : IProductCategoryRepository
{
    private readonly LidemiaDbContext context;

    public ProductCategoryRepository(LidemiaDbContext context)
    {
        this.context = context;
    }

    public Task<ProductCategoryEntity?> GetById(long key, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Delete(long key, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<ProductCategoryEntity>> GetCategories(CancellationToken cancellationToken)
    {
        return await this.context.ProductCategories.Include(x => x.Parent).AsActive().ToListAsync(cancellationToken: cancellationToken);
    }
}