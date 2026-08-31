// Created on 01/09/2026 16:37 by Laserson

using Lidemia.Core.Extensions;
using Lidemia.DataAccess;

namespace Lidemia.Logic.Extensions;

public static class ModulesExtensions
{
    public static void RegisterModules(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDataAccessModule(configuration.GetConnectionString("Main").NotNull());
    }
}