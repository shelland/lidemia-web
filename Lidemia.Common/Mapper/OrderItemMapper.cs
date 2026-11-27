// Created on 18/11/2026 21:52 by Laserson

using Lidemia.Common.Mapper.Abstract;
using Lidemia.Core.Models.Domain;
using Lidemia.DataAccess.Entities;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.Common.Mapper;

public interface IOrderItemMapper : IMapper<OrderItemEntity, OrderItemModel>;

[ServiceDescriptor<IOrderItemMapper>(ServiceLifetime.Scoped)]
public class OrderItemMapper : IOrderItemMapper
{
    private readonly IProductMapper productMapper;

    public OrderItemMapper(IProductMapper productMapper)
    {
        this.productMapper = productMapper;
    }

    public OrderItemModel? Map(OrderItemEntity? source)
    {
        if (source == null)
        {
            return null;
        }

        return new OrderItemModel(
            Product: this.productMapper.Map(source.Product),
            Quantity: source.Quantity,
            PricePerItem: source.PricePerItem,
            PricePerItemDiscount: source.PricePerItemDiscount,
            DiscountSource: source.DiscountSource
        );
    }
}