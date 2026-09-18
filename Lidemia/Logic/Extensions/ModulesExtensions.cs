// Created on 01/09/2026 16:37 by Laserson

using Lidemia.Common;
using Lidemia.Common.BusinessLogic;
using Lidemia.DataAccess;
using Lidemia.EmailNotifications;
using Lidemia.Search;
using Lidemia.Web.BusinessLogic;

namespace Lidemia.Logic.Extensions;

public static class ModulesExtensions
{
    public static void RegisterModules(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDataAccessModule(configuration);
        services.AddCommonBusinessLogicModule();
        services.AddWebBusinessLogicModule();
        services.AddSearchModule(configuration);
        services.AddEmailNotificationsModule(configuration);
        services.AddCommonModule();
        services.AddWebModule();
    }
}