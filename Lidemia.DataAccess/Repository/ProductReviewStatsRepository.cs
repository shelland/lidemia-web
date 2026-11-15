// Created on 15/11/2026 14:14 by Laserson

using Lidemia.DataAccess.Context;
using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Entities.Misc;
using Lidemia.DataAccess.Repository.Abstract;
using LinqToDB;
using LinqToDB.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.DataAccess.Repository;

[ServiceDescriptor<IProductReviewStatsRepository>(ServiceLifetime.Scoped)]
public class ProductReviewStatsRepository : IProductReviewStatsRepository
{
    private readonly LidemiaDbContext context;

    public ProductReviewStatsRepository(LidemiaDbContext context)
    {
        this.context = context;
    }

    public async Task Recalculate(CancellationToken cancellationToken)
    {
        var groups = this.context.ProductReviews.GroupBy(x => x.ProductId);
        var offset = DateTimeOffset.UtcNow.AddMonths(-3);

        var entities = new List<ProductReviewStatsEntity>();

        foreach (var group in groups)
        {
            var average = group.Average(x => x.Rate);
            var recentAverage = group.Where(x => x.UpdateDate >= offset).Average(x => x.Rate);
            var currentDistributions = group.GroupBy(x => x.Rate).ToDictionary(x => x.Key, x => x.Count());

            var distributions = new Dictionary<byte, int>
            {
                [1] = currentDistributions.TryGetValue(1, out var rate1) ? rate1 : 0,
                [2] = currentDistributions.TryGetValue(2, out var rate2) ? rate2 : 0,
                [3] = currentDistributions.TryGetValue(3, out var rate3) ? rate3 : 0,
                [4] = currentDistributions.TryGetValue(4, out var rate4) ? rate4 : 0,
                [5] = currentDistributions.TryGetValue(5, out var rate5) ? rate5 : 0
            };

            entities.Add(new ProductReviewStatsEntity
            {
                ProductId = group.Key,
                AverageRatingOverall = average,
                AverageRatingRecent = recentAverage,
                TotalRates = group.Count(),
                Distribution = new ProductReviewRateDistributionDbModel(Distribution: distributions)
            });
        }

        await this.context.GetTable<ProductReviewStatsEntity>()
            .Merge()
            .Using(entities)
            .On((target, source) => target.ProductId == source.ProductId)
            .UpdateWhenMatched((target, source) => source)
            .InsertWhenNotMatched(source => source)
            .MergeAsync(cancellationToken);
    }
}