// Created on 02/10/2026 19:11 by Laserson

using Lidemia.Resources.App;
using System.ComponentModel.DataAnnotations;

namespace Lidemia.Core.Enums;

public enum DeliveryType
{
    [Display(Name = "EnumDeliveryTypeDelivery", ResourceType = typeof(AppResources))]
    Delivery = 1,

    [Display(Name = "EnumDeliveryTypeSelfPickup", ResourceType = typeof(AppResources))]
    SelfPickup = 2
}