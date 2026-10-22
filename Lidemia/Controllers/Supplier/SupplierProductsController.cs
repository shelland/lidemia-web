// Created on 03/09/2026 20:28 by Laserson

using FluentValidation;
using Lidemia.Common.BusinessLogic.Services.App.Abstract;
using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Core.Attributes;
using Lidemia.Core.Enums;
using Lidemia.Core.Extensions;
using Lidemia.Core.Models.Bus;
using Lidemia.Core.Models.Dto;
using Lidemia.Core.Models.Misc;
using Lidemia.Core.Models.Service;
using Lidemia.ViewModels;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Lidemia.Controllers.Supplier;

[SupplierAuthorize]
[Route("Supplier/Products")]
public class SupplierProductsController : BaseController
{
    private static readonly string[] AllowedImageExtensions = [".jpg", ".jpeg", ".png", ".gif", ".webp", ".bmp"];
    private readonly IProductService productService;
    private readonly IValidator<SaveProductRequestDto> validator;
    private readonly IObjectStorageService objectStorageService;
    private readonly IPublishEndpoint publishEndpoint;
    private readonly ILogger<SupplierProductsController> logger;

    public SupplierProductsController(
        IProductService productService,
        IValidator<SaveProductRequestDto> validator,
        IObjectStorageService objectStorageService,
        IPublishEndpoint publishEndpoint,
        ILogger<SupplierProductsController> logger)
    {
        this.productService = productService;
        this.validator = validator;
        this.objectStorageService = objectStorageService;
        this.publishEndpoint = publishEndpoint;
        this.logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] PagingInfoModel pagingInfo, CancellationToken cancellationToken)
    {
        var products = await this.productService.GetSupplierProducts(RequireEntityId, pagingInfo, cancellationToken);
        return View(products);
    }

    [HttpGet("Create")]
    public async Task<IActionResult> Create()
    {
        return View(new CreateProductViewModel(null));
    }

    [HttpGet("Edit/{id:long}")]
    public async Task<IActionResult> Edit([FromRoute] long id, CancellationToken cancellationToken)
    {
        var product = await this.productService.GetById(id, cancellationToken);

        if (product == null || product.SupplierId != RequireEntityId)
        {
            return RedirectToAction("Index");
        }

        var viewModel = new CreateProductViewModel(product);
        return View("Create", viewModel);
    }

    [HttpPost]
    public async Task<ResultInfo> Save([FromForm] SaveProductRequestDto request, [FromForm] List<IFormFile>? images, CancellationToken cancellationToken)
    {
        var validationResult = await this.validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return new ErrorResultInfo(validationResult.GetErrors());
        }

        var result = await productService.Save(RequireEntityId, request, cancellationToken);

        if (!result.IsSuccess)
        {
            return new ErrorResultInfo(result.Errors.Select(x => x.Message));
        }

        var uploadedImages = await this.UploadProductImages(result.Value, images, cancellationToken);

        if (uploadedImages.Any())
        {
            await this.publishEndpoint.Publish(
                new ProductPhotosUploadedBusEventModel(
                    ProductId: result.Value,
                    UserId: RequireUserId,
                    IsPrimary: false,
                    Results: uploadedImages),
                cancellationToken);
        }

        return new ResultInfo<IEnumerable<ObjectStorageUploadResult>>(uploadedImages);
    }

    [HttpPost("SetVisibility")]
    public async Task<ResultInfo> SetVisibility([FromBody] SetProductVisibilityRequestDto request, CancellationToken cancellationToken)
    {
        await this.productService.SetProductVisibility(RequireEntityId, request, cancellationToken);
        return ResultInfo.Ok;
    }

    private async Task<IReadOnlyList<ObjectStorageUploadResult>> UploadProductImages(long productId, List<IFormFile>? images,
        CancellationToken cancellationToken)
    {
        var uploads = new List<ObjectStorageUploadResult>();

        if (images is not { Count: > 0 })
        {
            return uploads;
        }

        var tags = new Dictionary<string, string>
        {
            ["product-id"] = productId.ToString()
        };

        foreach (var image in images)
        {
            if (image.Length == 0)
            {
                continue;
            }

            var extension = Path.GetExtension(image.FileName).ToLowerInvariant();

            if (!AllowedImageExtensions.Contains(extension))
            {
                this.logger.LogWarning("Image {Name} was skipped because of unsupported extension {Extension}", image.FileName, extension);
                continue;
            }

            var fileName = $"{Guid.NewGuid():N}{extension}";

            await using var content = image.OpenReadStream();

            var uploadResult = await this.objectStorageService.Upload(content, ObjectStoragePayloadType.ProductPhoto, fileName, tags, cancellationToken);
            uploads.Add(uploadResult);
        }

        this.logger.LogInformation("Uploaded {Count} images for product {ProductId}", uploads.Count, productId);

        return uploads;
    }
}