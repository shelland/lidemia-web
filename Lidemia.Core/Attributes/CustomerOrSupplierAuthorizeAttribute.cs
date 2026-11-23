// Created on 23/11/2026 19:46 by Laserson

using Microsoft.AspNetCore.Authorization;

namespace Lidemia.Core.Attributes;

public class CustomerOrSupplierAuthorizeAttribute : AuthorizeAttribute
{
    public CustomerOrSupplierAuthorizeAttribute() : base(Constants.AuthValues.CustomerOrSupplierPolicyName)
    {
    }
}