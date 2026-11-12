// Created on 12/11/2026 12:57 by Laserson

namespace Lidemia.Core.Models.Misc;

public record SupplierOrderItems
(
    ICollection<SupplierOrderItem> Items
)
{
    public decimal TotalSum()
    {
        return Items.Sum(item => item.DiscountPriceForSingle ?? item.PriceForSingle);
    }
};

public record SupplierOrderItem
(
    long ProductId,
    int Quantity,
    decimal PriceForSingle,
    decimal? DiscountPriceForSingle
);