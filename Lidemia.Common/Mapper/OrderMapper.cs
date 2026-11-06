// Created on 26/10/2026 20:22 by Laserson

using Lidemia.Common.Mapper.Abstract;
using Lidemia.Core.Models.Domain;
using Lidemia.DataAccess.Entities;
using Scrutor;

namespace Lidemia.Common.Mapper;

public interface IOrderMapper : IMapper<OrderEntity, OrderModel>;

[ServiceDescriptor<IOrderMapper>]
public class OrderMapper : IOrderMapper
{
    private readonly ICustomerMapper customerMapper;

    public OrderMapper(ICustomerMapper customerMapper)
    {
        this.customerMapper = customerMapper;
    }

    public OrderModel Map(OrderEntity source)
    {
        return new OrderModel
        {
            Id = source.Id,
            Number = source.Number,
            CreateDate = source.CreateDate,
            Customer = this.customerMapper.Map(source.Customer)
        };
    }
}