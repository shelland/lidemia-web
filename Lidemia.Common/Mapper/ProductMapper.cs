// Created on 26/10/2026 19:34 by Laserson

using Lidemia.Common.Mapper.Abstract;
using Lidemia.Core.Models.Domain;
using Lidemia.Core.Models.Misc;
using Lidemia.DataAccess.Entities;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.Common.Mapper;

public interface IProductMapper : IMapper<ProductEntity, ProductModel>;

public interface IProductMainInfoMapper : IMapper<ProductEntity, ProductMainInfoModel>;

[ServiceDescriptor<IProductMapper>(ServiceLifetime.Scoped)]
public class ProductMapper : IProductMapper
{
    private readonly IPhotoMapper photoMapper;
    private readonly IProductCategoryMapper productCategoryMapper;

    public ProductMapper(IPhotoMapper photoMapper, IProductCategoryMapper productCategoryMapper)
    {
        this.photoMapper = photoMapper;
        this.productCategoryMapper = productCategoryMapper;
    }

    public ProductModel Map(ProductEntity entity)
    {
        return new ProductModel
        {
            Id = entity.Id,
            SupplierId = entity.SupplierId,
            ParentId = entity.ParentId,
            Description = entity.Description,
            ShortDescription = entity.ShortDescription,
            Title = entity.Title,
            Tags = entity.Tags,
            Price = entity.Price,
            PriceUnit = entity.PriceUnit,
            AvailabilityType = entity.AvailabilityType,
            MinQuantity = (decimal?)entity.MinQuantity,
            MaxQuantity = (decimal?)entity.MaxQuantity,
            Sku = entity.Sku,
            DiscountPrice = entity.DiscountPrice,
            HasCurrentDiscount = entity.HasCurrentDiscount,
            DiscountStartDate = entity.DiscountStartDate,
            DiscountEndDate = entity.DiscountEndDate,
            PrimaryPhoto = entity.PrimaryPhoto != null ? this.photoMapper.Map(entity.PrimaryPhoto) : this.photoMapper.GetPlaceholder(),
            Slug = entity.Slug,
            Category = entity.Category != null ? this.productCategoryMapper.Map(entity.Category) : null,
            AverageRate = entity.AverageRatingOverall ?? 0,
            AverageRecentRate = entity.AverageRatingRecent ?? 0,
            TotalRates = entity.TotalRates,
            CreateDate = entity.CreateDate,
            UpdateDate = entity.UpdateDate
        };
    }
}

[ServiceDescriptor<IProductMainInfoMapper>(ServiceLifetime.Scoped)]
public class ProductMainInfoMapper : IProductMainInfoMapper
{
    public ProductMainInfoModel Map(ProductEntity source)
    {
        return new ProductMainInfoModel(
            Id: source.Id,
            Title: source.Title,
            Price: source.Price,
            SupplierId: source.SupplierId,
            Description: source.Description,
            CategoryId: source.CategoryId.GetValueOrDefault(),
            Tags: source.Tags,
            IsVisible: source.IsVisible
        );
    }
}