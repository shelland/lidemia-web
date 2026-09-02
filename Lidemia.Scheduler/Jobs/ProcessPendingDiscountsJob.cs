// Created on 02/09/2026 21:23 by Laserson

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Lidemia.Scheduler.Jobs;

public class ProcessPendingDiscountsJob : BackgroundService
{
    private readonly IServiceScopeFactory serviceScopeFactory;

    public ProcessPendingDiscountsJob(IServiceScopeFactory serviceScopeFactory)
    {
        this.serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromHours(1));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            await using var scope = this.serviceScopeFactory.CreateAsyncScope();
        }
    }
}