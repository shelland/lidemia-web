// Created on 30/10/2025 22:28 by Laserson

using Microsoft.AspNetCore.Authorization;

namespace Lidemia.Core.Attributes;

public class SupplierAuthorizeAttribute : AuthorizeAttribute
{
    public SupplierAuthorizeAttribute() : base(Constants.AuthValues.SupplierPolicyName)
    {
    }
}