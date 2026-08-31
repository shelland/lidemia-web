// Created on 05/03/2020 18:08 by Andrey Laserson

using Lidemia.Core.Enums;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Lidemia.Logic.Extensions;

public static class ServiceAuthExtensions
{
    public static void RegisterAuth(this IServiceCollection services, IConfiguration configuration)
    {
        // services.AddScoped<CookieAuthEvents>();

        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(opts =>
            {
                opts.LoginPath = "/SignIn";
                opts.ExpireTimeSpan = TimeSpan.FromDays(60);
                opts.SlidingExpiration = true;
                opts.Cookie.Name = Core.Constants.CookieName;
                opts.Cookie.Path = "/"; 
                opts.Cookie.SameSite = SameSiteMode.Lax;
                opts.Cookie.Domain = configuration.GetValue<string>("Application:CookieDomain");
                // opts.Events = services.BuildServiceProvider().GetRequiredService<CookieAuthEvents>();
            });

        services.AddAuthorization(opts =>
        {
            opts.AddPolicy(Core.Constants.AuthValues.AdminPolicyName, cfg =>
            {
                cfg.RequireClaim(Core.Constants.Claims.RoleClaimName, nameof(EntityType.Admin));
            });

            opts.AddPolicy(Core.Constants.AuthValues.SupplierPolicyName, cfg =>
            {
                cfg.RequireClaim(Core.Constants.Claims.RoleClaimName, nameof(EntityType.Supplier), nameof(EntityType.Admin));
            });

            opts.AddPolicy(Core.Constants.AuthValues.CustomerPolicyName, cfg =>
            {
                cfg.RequireClaim(Core.Constants.Claims.RoleClaimName, nameof(EntityType.Customer), nameof(EntityType.Admin));
            });
        });
    }
}