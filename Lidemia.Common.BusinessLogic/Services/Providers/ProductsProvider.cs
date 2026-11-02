// Created on 02/11/2026 22:21 by Laserson

using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Common.BusinessLogic.Services.Providers.Abstract;
using Lidemia.Core.Models.Domain;
using Lidemia.Core.Models.Dto;
using Lidemia.Core.Models.Misc;
using Lidemia.Search.Services.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.Common.BusinessLogic.Services.Providers;

[ServiceDescriptor<IProductsProvider>(ServiceLifetime.Scoped)]
public class ProductsProvider : IProductsProvider
{
    private readonly IProductService productService;
    private readonly ISearchService searchService;

    public ProductsProvider(IProductService productService, ISearchService searchService)
    {
        this.productService = productService;
        this.searchService = searchService;
    }

    public async Task<BasePagedListModel<ProductModel>> GetPublicList(ProductsListFilterModel filter, CancellationToken cancellationToken)
    {
        if (!filter.HasSearchParams())
        {
            return await this.productService.GetPublicList(filter, [], cancellationToken);
        }

        var searchResults = await this.searchService.Search(filter, cancellationToken);
        var ids = searchResults.Select(x => x.Id).ToArray();

        return await this.productService.GetPublicList(filter, ids, cancellationToken);
    }
}