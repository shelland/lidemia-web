// Created on 15/03/2020 20:26 by Andrey Laserson

using System.Security.Claims;
using Lidemia.Core.Enums;

namespace Lidemia.Core.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static long? GetEntityId(this ClaimsPrincipal claimsPrincipal)
    {
        var uidClaim = claimsPrincipal.FindFirst(x => x.Type == Constants.Claims.EntityIdClaimName);

        if (uidClaim == null)
        {
            return null;
        }

        return long.Parse(uidClaim.Value);
    }

    public static string? GetEntityName(this ClaimsPrincipal claimsPrincipal)
    {
        var userNameClaim = claimsPrincipal.FindFirst(x => x.Type == Constants.Claims.UserNameClaimName);
        return userNameClaim?.Value;
    }

    public static string? GetSessionToken(this ClaimsPrincipal claimsPrincipal)
    {
        var sessionClaim = claimsPrincipal.FindFirst(x => x.Type == Constants.Claims.SessionAccessTokenClaimName);
        return sessionClaim?.Value;
    }

    public static bool IsAdmin(this ClaimsPrincipal claimsPrincipal)
    {
        var claim = claimsPrincipal.FindFirst(x => x.Type == Constants.Claims.RoleClaimName).NotNull();
        return claim.Value == nameof(EntityType.Admin);
    }

    public static EntityType GetEntityRole(this ClaimsPrincipal claimsPrincipal)
    {
        var roleClaim = claimsPrincipal.FindFirst(x => x.Type == Constants.Claims.RoleClaimName).NotNull();
        return Enum.Parse<EntityType>(roleClaim.Value);
    }
}