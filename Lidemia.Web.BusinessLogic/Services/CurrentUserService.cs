// Created on 20/12/2021 23:16 by shell

using Lidemia.Core.Extensions;
using Lidemia.Core.Models.Misc;
using Lidemia.Web.BusinessLogic.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.Web.BusinessLogic.Services;

[ServiceDescriptor<ICurrentUserService>(ServiceLifetime.Singleton)]
public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }

    public async Task<CurrentEntityModel?> GetUser()
    {
        var currentEntityId = this.httpContextAccessor.HttpContext?.User.GetEntityId();

        if (currentEntityId == null)
        {
            return null;
        }

        var user = this.httpContextAccessor.HttpContext.NotNull().User;

        return await Task.FromResult(new CurrentEntityModel
        {
            Id = currentEntityId.Value,
            Name = user.GetEntityName().NotNull(),
            Role = user.GetEntityRole()
        });
    }
}