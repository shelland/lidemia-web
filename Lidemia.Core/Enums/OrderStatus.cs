// Created on 29/11/2021 23:15 by Andrey Laserson

using Lidemia.Resources.App;
using System.ComponentModel.DataAnnotations;

namespace Lidemia.Core.Enums;

public enum OrderStatus
{
    [Display(Name = "EnumOrderStatusCancelled", ResourceType = typeof(AppResources))]
    Cancelled = -1,

    [Display(Name = "EnumOrderStatusNew", ResourceType = typeof(AppResources))]
    New = 1,

    [Display(Name = "EnumOrderStatusConfirmed", ResourceType = typeof(AppResources))]
    Confirmed = 2,

    [Display(Name = "EnumOrderStatusInProgress", ResourceType = typeof(AppResources))]
    InProgress = 3,

    [Display(Name = "EnumOrderStatusShipping", ResourceType = typeof(AppResources))]
    Shipping = 4,

    [Display(Name = "EnumOrderStatusWaitingForPickup", ResourceType = typeof(AppResources))]
    WaitingForPickup = 5,

    [Display(Name = "EnumOrderStatusFinished", ResourceType = typeof(AppResources))]
    Finished = 6
}