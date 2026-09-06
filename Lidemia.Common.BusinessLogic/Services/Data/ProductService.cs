// Created on 04/09/2026 18:26 by Laserson

using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Common.Mapping;
using Lidemia.Core.Models.Domain;
using Lidemia.Core.Models.Dto;
using Lidemia.Core.Models.Misc;
using Lidemia.DataAccess.Repository.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.Common.BusinessLogic.Services.Data;

[ServiceDescriptor<IProductService>(ServiceLifetime.Scoped)]
public class ProductService : IProductService
{
    private readonly IProductRepository productRepository;

    public ProductService(IProductRepository productRepository)
    {
        this.productRepository = productRepository;
    }

    public async Task<ProductModel?> GetById(long id, CancellationToken cancellationToken)
    {
        var product = await this.productRepository.GetById(id, cancellationToken);

        if (product == null)
        {
            return null;
        }

        return product.ToModel();
    }

    public Task<BasePagedListModel<ProductModel>> GetPublicList(ProductsListFilterModel filter, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}