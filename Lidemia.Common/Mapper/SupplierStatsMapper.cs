// Created on 20/11/2026 17:28 by Laserson

using Lidemia.Common.Mapper.Abstract;
using Lidemia.Core.Models.Domain;
using Lidemia.DataAccess.Entities;
using Scrutor;

namespace Lidemia.Common.Mapper;

public interface ISupplierStatsMapper : IMapper<SupplierStatsEntity, SupplierStatsModel>;

[ServiceDescriptor<ISupplierStatsMapper>]
public class SupplierStatsMapper : ISupplierStatsMapper
{
    public SupplierStatsModel? Map(SupplierStatsEntity? source)
    {
        if (source == null)
        {
            return null;
        }

        return new SupplierStatsModel(
            TotalOrders: source.TotalOrders,
            AverageRating: source.AverageRating,
            TotalProducts: source.TotalProducts
        );
    }
}