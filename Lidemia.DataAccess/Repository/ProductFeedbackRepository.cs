// Created on 06/09/2026 20:16 by Laserson

using Lidemia.Core.Enums;
using Lidemia.DataAccess.Context;
using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Extensions;
using Lidemia.DataAccess.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.DataAccess.Repository;

[ServiceDescriptor<IProductFeedbackRepository>(ServiceLifetime.Scoped)]
public class ProductFeedbackRepository : IProductFeedbackRepository
{
    private readonly LidemiaDbContext context;

    public ProductFeedbackRepository(LidemiaDbContext context)
    {
        this.context = context;
    }

    public Task<ProductFeedbackEntity?> GetById(Guid key, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Delete(Guid key, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyList<ProductFeedbackEntity>> GetRecentFeedback(long productId, int count = 5, CancellationToken cancellationToken = default)
    {
        return await this.context.ProductFeedbacks
            .AsActive()
            .Where(x => x.ProductId == productId && x.State == ProductFeedbackState.Approved)
            .OrderByDescending(x => x.CreateDate)
            .Take(count)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}