// Created on 02/11/2026 17:34 by Laserson

using Lidemia.Admin.Logic.Services.Abstract;
using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Search.Services.Abstract;
using Scrutor;

namespace Lidemia.Admin.Logic.Services;

[ServiceDescriptor<ISearchIndexService>(ServiceLifetime.Singleton)]
public class SearchIndexService : ISearchIndexService
{
    private readonly ISearchService searchService;
    private readonly IProductService productService;

    public SearchIndexService(ISearchService searchService, IProductService productService)
    {
        this.searchService = searchService;
        this.productService = productService;
    }

    public async Task RebuildIndex(CancellationToken cancellationToken)
    {
        var products = await this.productService.GetProductsForSearchIndex(cancellationToken);
        await this.searchService.RebuildIndex(products, cancellationToken);
    }
}