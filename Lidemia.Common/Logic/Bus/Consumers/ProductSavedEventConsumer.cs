// Created on 05/10/2026 19:24 by Laserson

using Lidemia.Core.Extensions;
using Lidemia.Core.Models.Bus;
using Lidemia.Core.Models.Misc;
using Lidemia.DataAccess.Repository.Abstract;
using Lidemia.Search.Services.Abstract;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Lidemia.Common.Logic.Bus.Consumers;

public class ProductSavedEventConsumer : IConsumer<ProductSavedBusEventModel>
{
    private readonly ISearchService searchService;
    private readonly IProductRepository productRepository;

    private readonly ILogger<ProductSavedEventConsumer> logger;

    public ProductSavedEventConsumer(
        ISearchService searchService,
        IProductRepository productRepository,
        ILogger<ProductSavedEventConsumer> logger)
    {
        this.searchService = searchService;
        this.productRepository = productRepository;
        this.logger = logger;
    }

    public async Task Consume(ConsumeContext<ProductSavedBusEventModel> context)
    {
        await UpdateSearchIndex(context.Message.Id, context.CancellationToken);
    }

    private async Task UpdateSearchIndex(long id, CancellationToken cancellationToken)
    {
        var product = (await this.productRepository.GetById(id, cancellationToken)).NotNull();
        
        await this.searchService.UpdateInIndex(new ProductMainInfoModel(
            Id: id,
            Title: product.Title!,
            Price: product.Price,
            SupplierId: product.SupplierId,
            Description: product.Description!,
            CategoryId: product.CategoryId!.Value,
            Tags: product.Tags,
            IsVisible: product.IsVisible
        ), cancellationToken);

        this.logger.LogInformation("Product {Id} was updated in search index", id);
    }
}