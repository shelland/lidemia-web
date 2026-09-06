// Created on 29/12/2021 22:05 by shell

using Lidemia.Core.Models.Configuration;
using Lidemia.Core.Models.Misc;
using Lidemia.Web.BusinessLogic.Services.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Scrutor;

namespace Lidemia.Web.BusinessLogic.Services;

[ServiceDescriptor<IAppStateService>(ServiceLifetime.Singleton)]
public class AppStateService : IAppStateService
{
    private readonly IOptions<AppIdSettings> options;

    public AppStateService(IOptions<AppIdSettings> options)
    {
        this.options = options;
    }

    public string GetPrefixedKey()
    {
        return $"{this.options.Value.AppName}:{this.options.Value.Module}:{this.options.Value.Environment}";
    }

    public AppIdModel GetAppId()
    {
        var opts = this.options.Value;

        return new AppIdModel(
            AppName: opts.AppName,
            Module: opts.Module,
            Environment: opts.Environment,
            Version: opts.SiteVersion
        );
    }
}