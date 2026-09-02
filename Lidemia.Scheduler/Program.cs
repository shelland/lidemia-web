using Lidemia.Common.Logic.Logging;
using Lidemia.DataAccess;
using Lidemia.Scheduler.Jobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Lidemia.Scheduler;

internal class Program
{
    static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Services.AddHostedService<ProcessPendingDiscountsJob>();
        builder.Services.RegisterLogging(builder.Configuration);
        builder.Services.AddDataAccessModule(builder.Configuration);

        using var app = builder.Build();
        await app.RunAsync();
    }
}