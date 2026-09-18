// Created on 10/09/2026 19:26 by Laserson

using Lidemia.Resources.App;
using System.ComponentModel.DataAnnotations;

namespace Lidemia.Core.Enums;

public enum OrganizationType
{
    [Display(Name = "EnumOrganizationTypeCompany", ResourceType = typeof(AppResources))]
    Company,

    [Display(Name = "EnumOrganizationTypeIndividual", ResourceType = typeof(AppResources))]
    Individual,

    [Display(Name = "EnumOrganizationTypeSelfEmployed", ResourceType = typeof(AppResources))]
    SelfEmployed
}