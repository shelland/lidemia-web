// Created on 01/12/2025 20:38 by Laserson

using Microsoft.AspNetCore.Authorization;

namespace Lidemia.Core.Attributes;

public class CustomerAuthorizeAttribute : AuthorizeAttribute
{
    public CustomerAuthorizeAttribute() : base(Constants.AuthValues.CustomerPolicyName)
    {
    }
}