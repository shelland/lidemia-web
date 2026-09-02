// Created on 02/09/2026 20:07 by Laserson

using Lidemia.Core.Models.Domain;
using Lidemia.DataAccess.Entities;

namespace Lidemia.Common.Mapping;

public static class ProductMapping
{
    public static ProductModel ToModel(this ProductEntity entity)
    {
        return new ProductModel();
    }
}