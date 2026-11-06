// Created on 06/09/2026 20:16 by Laserson

using FluentResults;
using Lidemia.Core.Enums;
using Lidemia.Core.Models.Service;
using Lidemia.DataAccess.Context;
using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Extensions;
using Lidemia.DataAccess.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.DataAccess.Repository;

[ServiceDescriptor<IProductReviewRepository>(ServiceLifetime.Scoped)]
public class ProductReviewRepository : IProductReviewRepository
{
    private readonly LidemiaDbContext context;

    public ProductReviewRepository(LidemiaDbContext context)
    {
        this.context = context;
    }

    public Task<ProductReviewEntity?> GetById(long key, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Delete(long key, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<ProductReviewEntity>> GetRecentReviews(long productId, int count = 5, CancellationToken cancellationToken = default)
    {
        return await this.context.ProductReviews
            .AsActive()
            .Include(x => x.Customer)
            .ThenInclude(x => x.User)
            .Where(x => x.ProductId == productId && x.State == ProductReviewState.Approved)
            .OrderByDescending(x => x.CreateDate)
            .Take(count)
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<Result<long>> SaveReview(SaveProductReviewModel model, CancellationToken cancellationToken)
    {
        if (model.Id.HasValue)
        {
            await this.context.ProductReviews.Where(x => x.Id == model.Id)
                .ExecuteUpdateAsync(x => x
                        .SetProperty(e => e.Review, model.Review)
                        .SetProperty(e => e.PositiveReview, model.PositiveReview)
                        .SetProperty(e => e.NegativeReview, model.NegativeReview)
                        .SetProperty(e => e.Rate, model.Rate)
                        .SetProperty(e => e.IsAnonymous, model.IsAnonymous)
                        .SetProperty(e => e.UpdateDate, DateTime.UtcNow)
                        .SetProperty(e => e.RowVersion, e => e.RowVersion + 1),
                    cancellationToken: cancellationToken);

            return Result.Ok(model.Id.Value);
        }

        var entity = new ProductReviewEntity
        {
            CustomerId = model.CustomerId,
            IsAnonymous = model.IsAnonymous,
            NegativeReview = model.NegativeReview,
            PositiveReview = model.PositiveReview,
            Review = model.Review,
            ProductId = model.ProductId,
            Rate = model.Rate,
            State = ProductReviewState.Pending
        };

        this.context.ProductReviews.Add(entity);
        await this.context.SaveChangesAsync(cancellationToken);

        return Result.Ok(entity.Id);
    }

    public Task SetReviewState(long reviewId, ProductReviewState state, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<ProductReviewEntity?> GetUserReview(long customerId, long productId, CancellationToken cancellationToken)
    {
        return this.context.ProductReviews.AsActive()
            .FirstOrDefaultAsync(x => x.ProductId == productId && x.CustomerId == customerId, cancellationToken: cancellationToken);
    }
}