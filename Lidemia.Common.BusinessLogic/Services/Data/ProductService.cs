// Created on 04/09/2026 18:26 by Laserson

using FluentResults;
using Lidemia.Common.BusinessLogic.Services.App.Abstract;
using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Common.Logic;
using Lidemia.Common.Mapper;
using Lidemia.Common.Mapper.Abstract;
using Lidemia.Core.Extensions;
using Lidemia.Core.Models.Bus;
using Lidemia.Core.Models.Domain;
using Lidemia.Core.Models.Dto;
using Lidemia.Core.Models.Misc;
using Lidemia.Core.Models.Service;
using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Repository.Abstract;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Scrutor;

namespace Lidemia.Common.BusinessLogic.Services.Data;

[ServiceDescriptor<IProductService>(ServiceLifetime.Scoped)]
public class ProductService : BaseDataService, IProductService
{
    private readonly IProductRepository productRepository;
    private readonly IUtc utc;
    private readonly IPublishEndpoint publishEndpoint;
    private readonly ISanitizerService sanitizerService;
    private readonly IProductMapper productMapper;
    private readonly IProductMainInfoMapper productMainInfoMapper;
    private readonly ILogger<ProductService> logger;

    public ProductService(
        IProductRepository productRepository,
        IUtc utc,
        IPublishEndpoint publishEndpoint,
        ISanitizerService sanitizerService,
        IProductMapper productMapper,
        IProductMainInfoMapper productMainInfoMapper,
        ILogger<ProductService> logger)
    {
        this.productRepository = productRepository;
        this.utc = utc;
        this.publishEndpoint = publishEndpoint;
        this.sanitizerService = sanitizerService;
        this.productMapper = productMapper;
        this.productMainInfoMapper = productMainInfoMapper;
        this.logger = logger;
    }

    public async Task<ProductModel?> GetById(long id, CancellationToken cancellationToken)
    {
        var product = await this.productRepository.GetById(id, cancellationToken);
        return product != null ? this.productMapper.Map(product) : null;
    }

    public async Task<BasePagedListModel<ProductModel>> GetPublicList(ProductsListFilterModel filter, long[] searchIds,
        CancellationToken cancellationToken)
    {
        var products = await this.productRepository.GetPublicList(filter, searchIds, cancellationToken);

        return new BasePagedListModel<ProductModel>
        {
            CurrentPage = filter.Page,
            TotalCount = products.TotalItemCount,
            Items = products.Select(this.productMapper.Map),
            TotalPages = products.PageCount
        };
    }

    public async Task<BasePagedListModel<ProductModel>> GetSupplierProducts(long id, PagingInfoModel pagingInfo, CancellationToken cancellationToken)
    {
        var products = await this.productRepository.GetSupplierProducts(id, pagingInfo, cancellationToken);

        return new BasePagedListModel<ProductModel>
        {
            CurrentPage = pagingInfo.Page,
            Items = products.Select(this.productMapper.Map),
            TotalCount = products.TotalItemCount,
            TotalPages = products.TotalItemCount
        };
    }

    public async Task<Result<long>> Save(long supplierId, SaveProductRequestDto request, CancellationToken cancellationToken)
    {
        if (request.Id.HasValue)
        {
            await EnsureOwnership(request.Id.Value, supplierId, cancellationToken);
        }

        var model = new SaveProductModel(
            Id: request.Id,
            ParentId: request.ParentId,
            Title: this.sanitizerService.Sanitize(request.Title),
            Description: this.sanitizerService.Sanitize(request.Description),
            ShortDescription: this.sanitizerService.Sanitize(request.ShortDescription),
            AvailabilityType: request.AvailabilityType,
            Tags: request.Tags,
            PriceUnit: request.PriceUnit,
            Price: request.Price,
            DiscountPrice: request.DiscountPrice,
            HasCurrentDiscount: request.HasCurrentDiscount,
            DiscountStartDate: request.DiscountStartDate,
            DiscountEndDate: request.DiscountEndDate,
            MinQuantity: request.MinQuantity,
            MaxQuantity: request.MaxQuantity,
            Sku: request.Sku,
            PrimaryPhotoId: request.PrimaryPhotoId,
            Slug: SlugHelper.CreateTransliteratedSlug(request.Title)
        );

        var result = await this.productRepository.Save(supplierId, model, cancellationToken);
        await this.publishEndpoint.Publish(new ProductSavedBusEventModel(Id: result.Value), cancellationToken);

        return result;
    }

    public Task<Result<(int Enabled, int Disabled)>> EnablePendingDiscounts(CancellationToken cancellationToken)
    {
        var now = this.utc.Now;
        return this.productRepository.EnablePendingDiscounts(now, cancellationToken);
    }

    public async Task SetProductVisibility(long supplierId, SetProductVisibilityRequestDto request, CancellationToken cancellationToken)
    {
        await EnsureOwnership(request.ProductId, supplierId, cancellationToken);

        await this.productRepository.SetProductVisibility(request.ProductId, request.IsVisible, cancellationToken);
        this.logger.LogInformation("Product {Id} visibility was changed to {State}", request.ProductId, request.IsVisible);
    }

    public async Task<IEnumerable<ProductModel>> GetProducts(IEnumerable<long> ids, CancellationToken cancellationToken)
    {
        var products = await this.productRepository.GetProducts(ids, cancellationToken);
        return products.Select(this.productMapper.Map);
    }

    public Task<IReadOnlyList<LightProductModel>> GetRelatedProducts(long categoryId, long currentProductId, int count = 3, CancellationToken cancellationToken = default)
    {
        return this.productRepository.GetRelatedProducts(categoryId, currentProductId, count, cancellationToken);
    }

    public async Task<IEnumerable<ProductMainInfoModel>> GetProductsForSearchIndex(CancellationToken cancellationToken)
    {
        var products = await this.productRepository.GetProductsForSearchIndex(cancellationToken);
        return this.productMainInfoMapper.MapList(products);
    }

    private async Task EnsureOwnership(long productId, long supplierId, CancellationToken cancellationToken)
    {
        var product = (await this.productRepository.GetById(productId, cancellationToken)).NotNull();

        if (product.SupplierId != supplierId)
        {
            ThrowNotAuthorized();
        }
    }
}