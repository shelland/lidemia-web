// Created on 29/10/2026 19:03 by Laserson

using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Common.Mapper;
using Lidemia.Core.Models.Domain;
using Lidemia.DataAccess.Repository.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.Common.BusinessLogic.Services.Data;

[ServiceDescriptor<IProductCategoryService>(ServiceLifetime.Scoped)]
public class ProductCategoryService : IProductCategoryService
{
    private readonly IProductCategoryRepository productCategoryRepository;
    private readonly IProductCategoryMapper productCategoryMapper;

    public ProductCategoryService(IProductCategoryRepository productCategoryRepository, IProductCategoryMapper productCategoryMapper)
    {
        this.productCategoryRepository = productCategoryRepository;
        this.productCategoryMapper = productCategoryMapper;
    }

    public async Task<IEnumerable<ProductCategoryModel>> GetCategories(CancellationToken cancellationToken)
    {
        var items = await this.productCategoryRepository.GetCategories(cancellationToken);
        return items.Select(this.productCategoryMapper.Map);
    }
}