// Created on 02/09/2026 20:07 by Laserson

using Lidemia.Core.Models.Domain;
using Lidemia.DataAccess.Entities;

namespace Lidemia.Common.Mapping;

public static class SupplierMapping
{
    public static SupplierModel ToModel(this SupplierEntity entity)
    {
        return new SupplierModel
        {
            Id = entity.Id,
            Name = entity.Name,
            CreateDate = entity.CreateDate,
            User = entity.User.ToModel()
        };
    }
}