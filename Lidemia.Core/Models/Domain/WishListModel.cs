// Created on 27/11/2026 12:27 by Laserson

using Lidemia.Core.Models.Base;

namespace Lidemia.Core.Models.Domain;

public class WishListItemModel : AbstractEntity<long>
{
    public ProductModel Product { get; set; } = null!;
}