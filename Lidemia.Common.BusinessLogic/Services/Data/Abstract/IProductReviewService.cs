// Created on 02/09/2026 20:10 by Laserson

using FluentResults;
using Lidemia.Core.Models.Domain;
using Lidemia.Core.Models.Dto;

namespace Lidemia.Common.BusinessLogic.Services.Data.Abstract;

public interface IProductReviewService
{
    Task<IReadOnlyList<ProductReviewModel>> GetRecentReviews(long productId, int take = 5, CancellationToken cancellationToken = default);

    Task<Result<long>> Save(long customerId, SaveProductReviewRequestDto request, CancellationToken cancellationToken);

    Task<ProductReviewModel?> GetUserReview(long customerId, long productId, CancellationToken cancellationToken);
}