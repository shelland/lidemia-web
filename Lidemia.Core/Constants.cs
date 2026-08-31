// Created on 27/11/2021 19:43 by Andrey Laserson

namespace Lidemia.Core;

public class Constants
{
    public const string AppName = "Lidemia";

    public const string CookieName = "Lidemia";

    public const string GeoCookieName = "CoffeePlanOneGeo";

    public const int DefaultSrid = 4326;

    public const int DefaultCacheDuration = 86400;

    public const string OrderPrefix = "U";

    public const string CorrelationIdHeader = "X-Correlation-Id";

    public const string RecaptchaHeader = "App-Recaptcha-Token";

    public static class Claims
    {
        public const string UserNameClaimName = "UserNameClaim";

        public const string EmailClaimName = "EmailClaim";

        public const string EntityIdClaimName = "EntityIdClaim";

        public const string UserIdClaimName = "UserIdClaim";

        public const string RoleClaimName = "RoleClaim";

        public const string FullNameClaimName = "FullNameClaim";

        public const string SessionAccessTokenClaimName = "SessionAccessClaim";

        public const string UserRoleClaimValue = "User";

        public const string AdminRoleClaimValue = "Admin";

        public const string ModeratorRoleClaimValue = "Moderator";
    }

    public static class AuthValues
    {
        public const string AdminPolicyName = "AllowAdmin";

        public const string SupplierPolicyName = "AllowSupplier";

        public const string CustomerPolicyName = "AllowUser";
    }

    public static class Delays
    {
        public static TimeSpan OneMinute { get; } = TimeSpan.FromMinutes(1);

        public static TimeSpan ThirtyMinutes { get; } = TimeSpan.FromMinutes(30);

        public static TimeSpan OneHour { get; } = TimeSpan.FromHours(1);

        public static TimeSpan OneDay { get; } = TimeSpan.FromDays(1);
    }

    public static class ProductSearchFields
    {
        public const string Id = "id";

        public const string Title = "title";

        public const string Description = "description";

        public const string Category = "category";

        public const string Price = "price";

        public const string SupplierId = "supplierId";
    }
}