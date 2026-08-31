namespace Lidemia.Processor;

using Microsoft.Extensions.Hosting;

internal class Program
{
    public static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        using var app = builder.Build();
        await app.RunAsync();
    }
}