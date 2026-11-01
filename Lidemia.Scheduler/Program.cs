using Lidemia.Common;
using Lidemia.Common.BusinessLogic;
using Lidemia.Common.Logic.Logging;
using Lidemia.DataAccess;
using Lidemia.Scheduler.Jobs;
using Lidemia.Scheduler.Logic.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Lidemia.Scheduler;

internal class Program
{
    static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Services.AddHostedService<ProcessPendingDiscountsJob>();
        builder.Services.AddHostedService<RecalculateProductRatingsJob>();
        builder.Services.AddHostedService<CalculateSupplierStatsJob>();

        builder.Services.RegisterLogging(builder.Configuration);
        builder.Services.AddDataAccessModule(builder.Configuration);
        builder.Services.AddCommonBusinessLogicModule(builder.Configuration);
        builder.Services.RegisterBus(builder.Configuration);
        builder.Services.AddCommonModule();

        using var app = builder.Build();
        await app.RunAsync();
    }
}