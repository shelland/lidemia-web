// Created on 03/09/2026 18:56 by Laserson

using FluentResults;
using Lidemia.Core.Enums;
using Lidemia.Core.Models.Service;
using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Repository.Abstract.Base;

namespace Lidemia.DataAccess.Repository.Abstract;

public interface IProductReviewRepository : IRepository<ProductReviewEntity, long>
{
    Task<IEnumerable<ProductReviewEntity>> GetRecentReviews(long productId, int count = 5, CancellationToken cancellationToken = default);

    Task<Result<long>> SaveReview(SaveProductReviewModel model, CancellationToken cancellationToken);

    Task SetReviewState(long reviewId, ProductReviewState state, CancellationToken cancellationToken);

    Task<ProductReviewEntity?> GetUserReview(long customerId, long productId, CancellationToken cancellationToken);
}