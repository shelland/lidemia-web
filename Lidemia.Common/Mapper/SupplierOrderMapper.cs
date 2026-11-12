// Created on 10/11/2026 12:24 by Laserson

using Lidemia.Common.Mapper.Abstract;
using Lidemia.Core.Models.Domain;
using Lidemia.DataAccess.Entities;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.Common.Mapper;

public interface ISupplierOrderMapper : IMapper<SupplierOrderEntity, SupplierOrderModel>;

[ServiceDescriptor<ISupplierOrderMapper>(ServiceLifetime.Scoped)]
public class SupplierOrderMapper : ISupplierOrderMapper
{
    private readonly IOrderMapper orderMapper;

    public SupplierOrderMapper(IOrderMapper orderMapper)
    {
        this.orderMapper = orderMapper;
    }

    public SupplierOrderModel Map(SupplierOrderEntity source)
    {
        return new SupplierOrderModel
        {
            Id = source.Id,
            CreateDate = source.CreateDate,
            Status = source.Status,
            LastStatusUpdate = source.LastStatusUpdate,
            Order = this.orderMapper.Map(source.Order),
            Items = source.Items
        };
    }
}