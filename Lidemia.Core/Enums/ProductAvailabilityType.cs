// Created on 06/05/2020 11:12 by Andrey Laserson

using System.ComponentModel.DataAnnotations;
using Lidemia.Resources.App;

namespace Lidemia.Core.Enums;

public enum ProductAvailabilityType
{
    [Display(Name = "EnumsProductAvailabilityTypeInStock", ResourceType = typeof(AppResources))]
    InStock, 

    [Display(Name = "EnumsProductAvailabilityTypePreOrder", ResourceType = typeof(AppResources))]
    PreOrder, 

    [Display(Name = "EnumsProductAvailabilityTypeOutOfStock", ResourceType = typeof(AppResources))]
    OutOfStock
}