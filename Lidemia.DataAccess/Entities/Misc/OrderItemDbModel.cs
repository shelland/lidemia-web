// Created on 01/12/2025 21:39 by Laserson

namespace Lidemia.DataAccess.Entities.Misc;

public record OrderItemDbModel
(
    long ProductId,
    decimal Quantity,
    decimal ProductPrice
);