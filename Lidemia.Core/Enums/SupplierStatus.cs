// Created on 03/09/2026 20:51 by Laserson

using Lidemia.Resources.App;
using System.ComponentModel.DataAnnotations;

namespace Lidemia.Core.Enums;

public enum SupplierStatus
{
    [Display(Name = "EnumSupplierStatusNotVerified", ResourceType = typeof(AppResources))]
    NotVerified = 0,

    [Display(Name = "EnumSupplierStatusVerifying", ResourceType = typeof(AppResources))]
    Verifying = 1,

    [Display(Name = "EnumSupplierStatusActive", ResourceType = typeof(AppResources))]
    Active = 2,

    [Display(Name = "EnumSupplierStatusBlocked", ResourceType = typeof(AppResources))]
    Blocked = 3
}