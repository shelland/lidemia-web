// Created on 30/10/2026 19:52 by Laserson

using Lidemia.Common.Mapper.Abstract;
using Lidemia.Core.Models.Domain;
using Lidemia.DataAccess.Entities;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.Common.Mapper;

public interface IProductReviewsMapper : IMapper<ProductReviewEntity, ProductReviewModel>;

[ServiceDescriptor<IProductReviewsMapper>(ServiceLifetime.Scoped)]
public class ProductReviewsMapper : IProductReviewsMapper
{
    private readonly ILightCustomerMapper lightCustomerMapper;

    public ProductReviewsMapper(ILightCustomerMapper lightCustomerMapper)
    {
        this.lightCustomerMapper = lightCustomerMapper;
    }

    public ProductReviewModel Map(ProductReviewEntity source)
    {
        return new ProductReviewModel
        {
            Id = source.Id,
            Customer = this.lightCustomerMapper.Map(source.Customer),
            Review = source.Review,
            PositiveReview = source.PositiveReview,
            NegativeRate = source.NegativeReview,
            IsAnonymous = source.IsAnonymous,
            Rate = source.Rate,
            SupplierFeedbackDate = source.SupplierFeedbackDate,
            SupplierFeedback = source.SupplierFeedback,
            CreateDate = source.CreateDate,
            UpdateDate = source.UpdateDate
        };
    }
}