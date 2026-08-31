// Created on 23/09/2023 15:10 by shell

using Lidemia.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Lidemia.Core.Extensions;

public static class MvcExtensions
{
    public static IMvcBuilder AddJson(this IMvcBuilder builder)
    {
        builder.AddJsonOptions(opts =>
        {
            opts.JsonSerializerOptions.PropertyNameCaseInsensitive = CommonJsonOptions.Options.PropertyNameCaseInsensitive;
            opts.JsonSerializerOptions.NumberHandling = CommonJsonOptions.Options.NumberHandling;
            opts.JsonSerializerOptions.PropertyNamingPolicy = CommonJsonOptions.Options.PropertyNamingPolicy;

            foreach (var converter in CommonJsonOptions.Options.Converters)
            {
                opts.JsonSerializerOptions.Converters.Add(converter);
            }
        });

        return builder;
    }
}