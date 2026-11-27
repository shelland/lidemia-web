// Created on 16/11/2026 19:01 by Laserson

using Lidemia.Common.Mapper.Abstract;
using Lidemia.Core.Models.Domain;
using Lidemia.DataAccess.Entities;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.Common.Mapper;

public interface IProductReviewStatsMapper : IMapper<ProductReviewStatsEntity, ProductReviewStatsModel>;

[ServiceDescriptor<IProductReviewStatsMapper>(ServiceLifetime.Scoped)]
public class ProductReviewStatsMapper : IProductReviewStatsMapper
{
    public ProductReviewStatsModel? Map(ProductReviewStatsEntity? source)
    {
        if (source == null)
        {
            return null;
        }

        return new ProductReviewStatsModel(
            AverageRatingRecent: source.AverageRatingRecent,
            AverageRatingOverall: source.AverageRatingOverall,
            TotalRates: source.TotalRates,
            Distribution: source.Distribution.Distribution
        );
    }
}