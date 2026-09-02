// Created on 29/04/2020 19:01 by Andrey Laserson

using Lidemia.Resources.App;
using System.ComponentModel.DataAnnotations;

namespace Lidemia.Core.Enums;

public enum ProductPriceUnit
{
    [Display(Name = "EnumsProductPriceUnitItem", ResourceType = typeof(AppResources))]
    Item, 

    [Display(Name = "EnumsProductPriceUnitItemWeight", ResourceType = typeof(AppResources))]
    Weight, 

    [Display(Name = "EnumsProductPriceUnitItemLiter", ResourceType = typeof(AppResources))]
    Liter
}