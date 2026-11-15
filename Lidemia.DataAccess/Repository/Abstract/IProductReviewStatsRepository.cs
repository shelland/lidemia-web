// Created on 15/11/2026 14:13 by Laserson

namespace Lidemia.DataAccess.Repository.Abstract;

public interface IProductReviewStatsRepository
{
    Task Recalculate(CancellationToken cancellationToken);
}