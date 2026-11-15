// Created on 13/11/2026 15:36 by Laserson

namespace Lidemia.DataAccess.Entities.Misc;

public record ProductSpecificationDbModel
(
    IEnumerable<ProductSpecificationItemDbModel> Items
)
{
    public static ProductSpecificationDbModel Default => new([]);
};

public record ProductSpecificationItemDbModel
(
    string Name,
    string Value
);