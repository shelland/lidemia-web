// Created on 29/11/2021 23:15 by Andrey Laserson

using Lidemia.Resources.App;
using System.ComponentModel.DataAnnotations;

namespace Lidemia.Core.Enums;

public enum OrderStatus
{
    [Display(Name = "EnumOrderStatusCancelled", ResourceType = typeof(AppResources))]
    Cancelled,

    [Display(Name = "EnumOrderStatusNew", ResourceType = typeof(AppResources))]
    New,

    [Display(Name = "EnumOrderStatusConfirmed", ResourceType = typeof(AppResources))]
    Confirmed,

    [Display(Name = "EnumOrderStatusInProgress", ResourceType = typeof(AppResources))]
    InProgress,

    [Display(Name = "EnumOrderStatusShipping", ResourceType = typeof(AppResources))]
    Shipping,

    [Display(Name = "EnumOrderStatusFinished", ResourceType = typeof(AppResources))]
    Finished
}