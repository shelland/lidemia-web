// Created on 18/07/2023 20:47 by shell

using Lidemia.Common.Logic.Abstract;
using Lidemia.Core.Models.Configuration;
using Lidemia.Core.Models.Misc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Scrutor;

namespace Lidemia.Common.Logic;

[ServiceDescriptor<IAppIdService>(ServiceLifetime.Singleton)]
public class AppIdService : IAppIdService
{
    private readonly IOptions<AppIdSettings> options;

    public AppIdService(IOptions<AppIdSettings> options)
    {
        this.options = options;
    }

    public string GetPrefixedKey()
    {
        // return $"{this.options.Value.AppName}:{this.options.Value.Module}:{this.options.Value.Environment}";
        return $"{this.options.Value.AppName}:{this.options.Value.Environment}";
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