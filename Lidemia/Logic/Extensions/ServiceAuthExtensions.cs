// Created on 05/03/2020 18:08 by Andrey Laserson

using Lidemia.Core;
using Lidemia.Core.Enums;
using Lidemia.Core.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Net;

namespace Lidemia.Logic.Extensions;

public static class ServiceAuthExtensions
{
    public static void RegisterAuth(this IServiceCollection services, IConfiguration configuration)
    {
        // services.AddScoped<CookieAuthEvents>();

        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(opts =>
            {
                // opts.LoginPath = "/SignIn";
                opts.ExpireTimeSpan = TimeSpan.FromDays(180);
                opts.SlidingExpiration = true;
                opts.Cookie.Name = Constants.CookieName;
                opts.Cookie.Path = "/";
                opts.Cookie.SameSite = SameSiteMode.Lax;
                opts.Cookie.Domain = configuration.GetValue<string>("Application:CookieDomain");
                opts.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = (ctx) =>
                    {
                        var generator = ctx.HttpContext.RequestServices.GetRequiredService<LinkGenerator>();

                        var endpoint = ctx.HttpContext.GetEndpoint()!;
                        var authorizeData = endpoint.Metadata.GetOrderedMetadata<IAuthorizeData>();

                        var policies = authorizeData
                            .Where(a => !string.IsNullOrEmpty(a.Policy))
                            .Select(a => a.Policy)
                            .First();

                        var returnUrl = $"{ctx.Request.Path}{ctx.Request.QueryString.Value}";
                        var redirectBaseUrl = policies == Constants.AuthValues.SupplierPolicyName
                            ? generator.GetPathByAction(ctx.HttpContext, "Index", "SupplierSignIn").NotNull()
                            : generator.GetPathByAction(ctx.HttpContext, "Index", "CustomerSignIn").NotNull();

                        var resultUrl = $"{redirectBaseUrl}?returnUrl={WebUtility.UrlEncode(returnUrl)}";
                        ctx.Response.Redirect(resultUrl);

                        return Task.CompletedTask;
                    },
                };
                // opts.Events = services.BuildServiceProvider().GetRequiredService<CookieAuthEvents>();
            });


        services.AddAuthorizationBuilder()
            .AddPolicy(Constants.AuthValues.AdminPolicyName,
                cfg => cfg.RequireClaim(Constants.Claims.RoleClaimName, nameof(EntityType.Admin)))
            .AddPolicy(Constants.AuthValues.SupplierPolicyName,
                cfg => cfg.RequireClaim(Constants.Claims.RoleClaimName, nameof(EntityType.Supplier), nameof(EntityType.Admin)))
            .AddPolicy(Constants.AuthValues.CustomerPolicyName,
                cfg => cfg.RequireClaim(Constants.Claims.RoleClaimName, nameof(EntityType.Customer), nameof(EntityType.Admin)));
    }
}