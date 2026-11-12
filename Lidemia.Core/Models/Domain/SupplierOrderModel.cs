// Created on 10/11/2026 12:25 by Laserson

using Lidemia.Core.Enums;
using Lidemia.Core.Models.Base;
using Lidemia.Core.Models.Misc;

namespace Lidemia.Core.Models.Domain;

public class SupplierOrderModel : AbstractEntity<long>
{
    public OrderStatus Status { get; set; }

    public DateTimeOffset? LastStatusUpdate { get; set; }

    public OrderModel Order { get; set; } = null!;

    public SupplierOrderItems Items { get; set; } = null!;
}