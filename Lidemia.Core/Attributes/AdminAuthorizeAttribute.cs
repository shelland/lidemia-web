// Created on 28/10/2025 21:41 by Laserson

using Microsoft.AspNetCore.Authorization;

namespace Lidemia.Core.Attributes;

public class AdminAuthorizeAttribute : AuthorizeAttribute
{
    public AdminAuthorizeAttribute() : base(Constants.AuthValues.AdminPolicyName)
    {
    }
}