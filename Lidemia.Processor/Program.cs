using Lidemia.Common.Logic.Bus;
using Lidemia.Common.Logic.Logging;

namespace Lidemia.Processor;

using Microsoft.Extensions.Hosting;

internal class Program
{
    public static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Services.RegisterBus(builder.Configuration);
        // builder.Services.AddEmailNotificationsModule(builder.Configuration);
        builder.Services.RegisterLogging(builder.Configuration);

        using var app = builder.Build();
        await app.RunAsync();
    }
}