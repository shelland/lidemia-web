// Created on 10/11/2026 12:28 by Laserson

using Lidemia.Core.Enums;
using Lidemia.Core.Models.Misc;

namespace Lidemia.Core.Models.Dto;

public class SupplierOrdersFilterModel : PagingInfoModel
{
    public OrderStatus? Status { get; set; }
}