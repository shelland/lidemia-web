// Created on 27/11/2026 12:22 by Laserson

using Lidemia.Core.Models.Misc;

namespace Lidemia.Core.Models.Dto;

public class CustomerWishListFilterModel : PagingInfoModel
{
    public long? ListId { get; set; }
}