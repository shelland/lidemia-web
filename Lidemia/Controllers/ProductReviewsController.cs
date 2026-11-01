// Created on 30/10/2026 20:14 by Laserson

using FluentValidation;
using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Core;
using Lidemia.Core.Attributes;
using Lidemia.Core.Extensions;
using Lidemia.Core.Models.Dto;
using Lidemia.Core.Models.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Lidemia.Controllers;

[Route("ProductReviews")]
public class ProductReviewsController : BaseController
{
    private readonly IProductReviewService productReviewService;
    private readonly IValidator<SaveProductReviewRequestDto> validator;

    public ProductReviewsController(IProductReviewService productReviewService, IValidator<SaveProductReviewRequestDto> validator)
    {
        this.productReviewService = productReviewService;
        this.validator = validator;
    }

    [HttpPost]
    [EnableRateLimiting(Constants.RateLimitingPolicies.ProductReviewsPolicy)]
    [CustomerAuthorize]
    public async Task<ResultInfo> Save(SaveProductReviewRequestDto request, CancellationToken cancellationToken)
    {
        var validationResult = await this.validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return new ErrorResultInfo(validationResult.GetErrors());
        }

        var result = await this.productReviewService.Save(RequireEntityId, request, cancellationToken);
        return result.IsFailed ? new ErrorResultInfo() : ResultInfo.Ok;
    }
}