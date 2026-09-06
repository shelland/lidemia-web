// Created on 29/04/2020 18:11 by Andrey Laserson

using System.Security.Claims;
using Lidemia.Core.Enums;
using Lidemia.Core.Extensions;
using Lidemia.Core.Models.Domain;
using Lidemia.Core.Models.Dto;
using Lidemia.DataAccess.Repository.Abstract;
using Lidemia.Web.BusinessLogic.Services.Abstract;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Scrutor;

namespace Lidemia.Web.BusinessLogic.Services;

[ServiceDescriptor<IWebAuthService>(ServiceLifetime.Transient)]
public class WebAuthService : IWebAuthService
{
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IAuthTokenRepository authTokenRepository;
    private readonly ILogger<WebAuthService> logger;

    public WebAuthService(
        IHttpContextAccessor httpContextAccessor,
        IAuthTokenRepository authTokenRepository,
        ILogger<WebAuthService> logger)
    {
        this.httpContextAccessor = httpContextAccessor;
        this.authTokenRepository = authTokenRepository;
        this.logger = logger;
    }

    public async Task AuthCustomer(SignInResult<CustomerModel> result, CancellationToken cancellationToken)
    {
        var sessionToken = Guid.NewGuid().ToString("N");
        var customer = result.Entity.NotNull();

        var claims = new List<Claim>
        {
            new(Core.Constants.Claims.EmailClaimName, customer.User.Email),
            // new(Core.Constants.Claims.FullNameClaimName, customer.Name),
            new(Core.Constants.Claims.EntityIdClaimName, customer.Id.ToString()),
            new(Core.Constants.Claims.UserIdClaimName, customer.User.Id.ToString()),
            new(Core.Constants.Claims.SessionAccessTokenClaimName, sessionToken),

            // This claim is required for SignalR hubs to identify a user
            new(ClaimTypes.NameIdentifier, customer.Id.ToString()),
            new(Core.Constants.Claims.RoleClaimName, nameof(EntityType.Customer))
        };

        var claimsIdentity = new ClaimsIdentity(
            claims,
            "ApplicationCookie",
            Core.Constants.Claims.UserNameClaimName,
            Core.Constants.Claims.RoleClaimName);

        await this.authTokenRepository.Create(customer.Id, sessionToken, EntityType.Customer, cancellationToken);

        this.logger.LogInformation("A new session was created for customer {User}", customer.Id);

        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        await this.httpContextAccessor.HttpContext!.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            claimsPrincipal, new AuthenticationProperties
            {
                IsPersistent = true
            });
    }

    public async Task AuthSupplier(SignInResult<SupplierModel> result, CancellationToken cancellationToken)
    {
        var sessionToken = Guid.NewGuid().ToString("N");
        var supplier = result.Entity.NotNull();

        var claims = new List<Claim>
        {
            new(Core.Constants.Claims.EmailClaimName, supplier.User.Email),
            new(Core.Constants.Claims.FullNameClaimName, supplier.FullName),
            new(Core.Constants.Claims.UserNameClaimName, supplier.ShortName),
            new(Core.Constants.Claims.EntityIdClaimName, supplier.Id.ToString()),
            new(Core.Constants.Claims.UserIdClaimName, supplier.User.Id.ToString()),
            new(Core.Constants.Claims.SessionAccessTokenClaimName, sessionToken),

            // This claim is required for SignalR hubs to identify a user
            new(ClaimTypes.NameIdentifier, supplier.Id.ToString()),
            new(Core.Constants.Claims.RoleClaimName, nameof(EntityType.Supplier))
        };

        var claimsIdentity = new ClaimsIdentity(
            claims,
            "ApplicationCookie",
            Core.Constants.Claims.UserNameClaimName,
            Core.Constants.Claims.RoleClaimName);

        await this.authTokenRepository.Create(supplier.Id, sessionToken, EntityType.Supplier, cancellationToken);

        this.logger.LogInformation("A new session was created for supplier {User}", supplier.Id);

        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        await this.httpContextAccessor.HttpContext!.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            claimsPrincipal, new AuthenticationProperties
            {
                IsPersistent = true,
            });
    }

    public async Task AuthAdmin(UserModel user, CancellationToken cancellationToken)
    {
        var sessionToken = Guid.NewGuid().ToString("N");

        var claims = new List<Claim>
        {
            new(Core.Constants.Claims.EmailClaimName, user.Email),
            new(Core.Constants.Claims.FullNameClaimName, "Admin"),
            new(Core.Constants.Claims.UserNameClaimName, "Admin"),
            new(Core.Constants.Claims.EntityIdClaimName, user.Id.ToString()),
            new(Core.Constants.Claims.UserIdClaimName, user.Id.ToString()),
            new(Core.Constants.Claims.SessionAccessTokenClaimName, sessionToken),

            // This claim is required for SignalR hubs to identify a user
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(Core.Constants.Claims.RoleClaimName, nameof(EntityType.Admin))
        };

        var claimsIdentity = new ClaimsIdentity(
            claims,
            "ApplicationCookie",
            Core.Constants.Claims.UserNameClaimName,
            Core.Constants.Claims.RoleClaimName);

        await this.authTokenRepository.Create(user.Id, sessionToken, EntityType.Supplier, cancellationToken);

        this.logger.LogInformation("A new ADMIN session was created for supplier {User}", user.Id);

        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        await this.httpContextAccessor.HttpContext!.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            claimsPrincipal, new AuthenticationProperties
            {
                IsPersistent = true,
            });
    }

    public async Task SignOut(CancellationToken cancellationToken)
    {
        var sessionToken = this.httpContextAccessor.HttpContext!.User.GetSessionToken();
        await this.httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        this.logger.LogInformation("Removing a session {Session} by logout request", sessionToken);
        await this.authTokenRepository.Delete(sessionToken!, cancellationToken);
    }
}