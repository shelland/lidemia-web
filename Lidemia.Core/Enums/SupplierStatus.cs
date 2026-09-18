// Created on 03/09/2026 20:51 by Laserson

using Lidemia.Resources.App;
using System.ComponentModel.DataAnnotations;

namespace Lidemia.Core.Enums;

public enum SupplierStatus
{
    [Display(Name = "EnumSupplierStatusNotVerified", ResourceType = typeof(AppResources))]
    NotVerified,

    [Display(Name = "EnumSupplierStatusVerifying", ResourceType = typeof(AppResources))]
    Verifying,

    [Display(Name = "EnumSupplierStatusActive", ResourceType = typeof(AppResources))]
    Active,

    [Display(Name = "EnumSupplierStatusBlocked", ResourceType = typeof(AppResources))]
    Blocked
}