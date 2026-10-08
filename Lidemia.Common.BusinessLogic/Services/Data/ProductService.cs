// Created on 04/09/2026 18:26 by Laserson

using FluentResults;
using Lidemia.Common.BusinessLogic.Services.App.Abstract;
using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Common.Mapping;
using Lidemia.Core.Enums;
using Lidemia.Core.Exceptions;
using Lidemia.Core.Extensions;
using Lidemia.Core.Models.Bus;
using Lidemia.Core.Models.Domain;
using Lidemia.Core.Models.Dto;
using Lidemia.Core.Models.Misc;
using Lidemia.Core.Models.Service;
using Lidemia.DataAccess.Repository.Abstract;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Scrutor;

namespace Lidemia.Common.BusinessLogic.Services.Data;

[ServiceDescriptor<IProductService>(ServiceLifetime.Scoped)]
public class ProductService : IProductService
{
    private readonly IProductRepository productRepository;
    private readonly IUtc utc;
    private readonly IPublishEndpoint publishEndpoint;
    private readonly ILogger<ProductService> logger;

    public ProductService(IProductRepository productRepository, IUtc utc, IPublishEndpoint publishEndpoint, ILogger<ProductService> logger)
    {
        this.productRepository = productRepository;
        this.utc = utc;
        this.publishEndpoint = publishEndpoint;
        this.logger = logger;
    }

    public async Task<ProductModel?> GetById(long id, CancellationToken cancellationToken)
    {
        var product = await this.productRepository.GetById(id, cancellationToken);
        return product?.ToModel();
    }

    public async Task<BasePagedListModel<ProductModel>> GetPublicList(ProductsListFilterModel filter, CancellationToken cancellationToken)
    {
        var products = await this.productRepository.GetPublicList(filter, cancellationToken);

        return new BasePagedListModel<ProductModel>
        {
            CurrentPage = filter.Page,
            TotalCount = products.TotalItemCount,
            Items = products.Select(x => x.ToModel()),
            TotalPages = products.PageCount
        };
    }

    public async Task<BasePagedListModel<ProductModel>> GetSupplierProducts(long id, PagingInfoModel pagingInfo, CancellationToken cancellationToken)
    {
        var products = await this.productRepository.GetSupplierProducts(id, pagingInfo, cancellationToken);

        return new BasePagedListModel<ProductModel>
        {
            CurrentPage = pagingInfo.Page,
            Items = products.Select(x => x.ToModel()),
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
            Title: request.Title,
            Description: request.Description,
            ShortDescription: request.ShortDescription,
            AvailabilityType: request.AvailabilityType
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

    private async Task EnsureOwnership(long productId, long supplierId, CancellationToken cancellationToken)
    {
        var product = (await this.productRepository.GetById(productId, cancellationToken)).NotNull();

        if (product.SupplierId != supplierId)
        {
            throw new AppFlowException(AppFlowExceptionType.Unauthorized);
        }
    }
}