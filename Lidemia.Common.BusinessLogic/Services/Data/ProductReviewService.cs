// Created on 03/09/2026 18:55 by Laserson

using FluentResults;
using Lidemia.Common.BusinessLogic.Services.App.Abstract;
using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Common.Mapper;
using Lidemia.Common.Mapper.Abstract;
using Lidemia.Core;
using Lidemia.Core.Models.Domain;
using Lidemia.Core.Models.Dto;
using Lidemia.Core.Models.Service;
using Lidemia.DataAccess.Repository.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Scrutor;

namespace Lidemia.Common.BusinessLogic.Services.Data;

[ServiceDescriptor<IProductReviewService>(ServiceLifetime.Scoped)]
public class ProductReviewService : IProductReviewService
{
    private readonly IProductReviewRepository productReviewRepository;
    private readonly IProductReviewsMapper productReviewsMapper;
    private readonly IOrderService orderService;
    private readonly ISanitizerService sanitizerService;
    private readonly ILogger<ProductReviewService> logger;

    public ProductReviewService(
        IProductReviewRepository productReviewRepository,
        IProductReviewsMapper productReviewsMapper,
        IOrderService orderService,
        ISanitizerService sanitizerService,
        ILogger<ProductReviewService> logger)
    {
        this.productReviewRepository = productReviewRepository;
        this.productReviewsMapper = productReviewsMapper;
        this.orderService = orderService;
        this.sanitizerService = sanitizerService;
        this.logger = logger;
    }

    public async Task<IReadOnlyList<ProductReviewModel>> GetRecentReviews(long productId, int take = 5, CancellationToken cancellationToken = default)
    {
        var reviews = await this.productReviewRepository.GetRecentReviews(productId, take, cancellationToken);
        return this.productReviewsMapper.MapList(reviews);
    }

    public async Task<Result<long>> Save(long customerId, SaveProductReviewRequestDto request, CancellationToken cancellationToken)
    {
        var lastOrder = await this.orderService.GetUserProductLastOrder(customerId, request.ProductId, cancellationToken);

        if (lastOrder == null)
        {
            this.logger.LogWarning("Unauthorized review for product {ProductId} from {CustomerId}", request.ProductId, customerId);
            return Result.Fail(Errors.UnauthorizedError);
        }

        var model = new SaveProductReviewModel(
            Id: request.Id,
            CustomerId: customerId,
            ProductId: request.ProductId,
            Review: this.sanitizerService.Sanitize(request.Review)!,
            PositiveReview: this.sanitizerService.Sanitize(request.PositiveReview),
            NegativeReview: this.sanitizerService.Sanitize(request.NegativeReview),
            Rate: request.Rate,
            IsAnonymous: request.IsAnonymous
        );

        this.logger.LogInformation("Saving product review for {ProductId} from {CustomerId}", request.ProductId, customerId);
        return await this.productReviewRepository.SaveReview(model, cancellationToken);
    }

    public async Task<ProductReviewModel?> GetUserReview(long customerId, long productId, CancellationToken cancellationToken)
    {
        var review = await this.productReviewRepository.GetUserReview(customerId, productId, cancellationToken);
        return review != null ? this.productReviewsMapper.Map(review) : null;
    }
}