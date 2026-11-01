// Created on 26/10/2026 20:04 by Laserson

using Lidemia.Common.Mapper.Abstract;
using Lidemia.Core.Models.Domain;
using Lidemia.DataAccess.Entities;
using Lidemia.Resources.App;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using System.Resources;

namespace Lidemia.Common.Mapper;

public interface IProductCategoryMapper : IMapper<ProductCategoryEntity, ProductCategoryModel>;

[ServiceDescriptor<IProductCategoryMapper>(ServiceLifetime.Scoped)]
public class ProductCategoryMapper : IProductCategoryMapper
{
    public ProductCategoryModel Map(ProductCategoryEntity source)
    {
        var manager = new ResourceManager(typeof(AppResources));

        return new ProductCategoryModel
        {
            Id = source.Id,
            CreateDate = source.CreateDate,
            Name = manager.GetString(source.NameKey) ?? source.NameKey,
            Description = source.DescriptionKey != null ? manager.GetString(source.DescriptionKey) : source.DescriptionKey,
            Parent = source.Parent != null
                ? new ProductCategoryModel
                {
                    Id = source.Parent.Id,
                    Name = manager.GetString(source.Parent.NameKey) ?? source.Parent.NameKey,
                    Description = source.Parent.DescriptionKey != null ? manager.GetString(source.Parent.DescriptionKey) : source.Parent.DescriptionKey,
                    CreateDate = source.Parent.CreateDate,
                }
                : null
        };
    }
}