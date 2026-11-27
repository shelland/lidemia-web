// Created on 26/10/2026 20:22 by Laserson

using Lidemia.Common.Mapper.Abstract;
using Lidemia.Core.Extensions;
using Lidemia.Core.Models.Domain;
using Lidemia.DataAccess.Entities;
using Scrutor;

namespace Lidemia.Common.Mapper;

public interface IOrderMapper : IMapper<OrderEntity, OrderModel>;

[ServiceDescriptor<IOrderMapper>]
public class OrderMapper : IOrderMapper
{
    private readonly ICustomerMapper customerMapper;
    private readonly IOrderItemMapper orderItemMapper;

    public OrderMapper(ICustomerMapper customerMapper, IOrderItemMapper orderItemMapper)
    {
        this.customerMapper = customerMapper;
        this.orderItemMapper = orderItemMapper;
    }

    public OrderModel? Map(OrderEntity? source)
    {
        if (source == null)
        {
            return null;
        }

        return new OrderModel
        {
            Id = source.Id,
            Number = source.Number,
            CreateDate = source.CreateDate,
            Customer = this.customerMapper.Map(source.Customer),
            TotalSum = source.TotalSum.ToPrice(),
            TotalSumDiscount = source.TotalSumDiscount?.ToPrice(),
            Items = this.orderItemMapper.MapList(source.Items)
        };
    }
}