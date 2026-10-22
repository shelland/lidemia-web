using Lidemia.Common.BusinessLogic;
using Lidemia.Common.Logic.Bus;
using Lidemia.Common.Logic.Logging;
using Lidemia.DataAccess;
using Lidemia.Search;

namespace Lidemia.Processor;

using Microsoft.Extensions.Hosting;

internal class Program
{
    public static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Services.RegisterBusWithConsumers(builder.Configuration);
        // builder.Services.AddEmailNotificationsModule(builder.Configuration);
        builder.Services.RegisterLogging(builder.Configuration);
        builder.Services.AddSearchModule(builder.Configuration);
        builder.Services.AddCommonBusinessLogicModule(builder.Configuration);
        builder.Services.AddDataAccessModule(builder.Configuration);
        using var app = builder.Build();
        await app.RunAsync();
    }
}