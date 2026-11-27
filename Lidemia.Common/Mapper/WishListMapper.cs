// Created on 27/11/2026 12:26 by Laserson

using Lidemia.Common.Mapper.Abstract;
using Lidemia.Core.Extensions;
using Lidemia.Core.Models.Domain;
using Lidemia.DataAccess.Entities;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.Common.Mapper;

public interface IWishListItemMapper : IMapper<WishListItemEntity, WishListItemModel>;

[ServiceDescriptor<IWishListItemMapper>(ServiceLifetime.Scoped)]
public class WishListItemMapper : IWishListItemMapper
{
    private readonly IProductMapper productMapper;

    public WishListItemMapper(IProductMapper productMapper)
    {
        this.productMapper = productMapper;
    }

    public WishListItemModel? Map(WishListItemEntity? source)
    {
        if (source == null)
        {
            return null;
        }

        return new WishListItemModel
        {
            Id = source.Id,
            Product = this.productMapper.Map(source.Product).NotNull(),
            CreateDate = source.CreateDate,
            UpdateDate = source.UpdateDate
        };
    }
}