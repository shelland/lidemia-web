// Created on 22/12/2021 12:10 by shell

using StackExchange.Profiling;

namespace Lidemia.Logic.Extensions;

public static class ProfilerExtensions
{
    public static IServiceCollection AddProfiler(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddMiniProfiler(opts =>
        {
            opts.IgnorePath("/lib");
            opts.IgnorePath("/css");
            opts.IgnorePath("/js");
            opts.IgnorePath("/img");
            opts.IgnorePath("/hubs");
            opts.IgnorePath("/fonts");
        }).AddEntityFramework();

        return serviceCollection;
    }
}