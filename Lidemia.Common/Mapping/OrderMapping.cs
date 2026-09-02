// Created on 02/09/2026 20:06 by Laserson

using Lidemia.Core.Models.Domain;
using Lidemia.DataAccess.Entities;

namespace Lidemia.Common.Mapping;

public static class OrderMapping
{
    public static OrderModel ToModel(this OrderEntity entity)
    {
        return new OrderModel();
    }
}