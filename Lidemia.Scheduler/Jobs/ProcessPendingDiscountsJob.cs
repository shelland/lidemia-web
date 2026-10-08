// Created on 02/09/2026 21:23 by Laserson

using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Lidemia.Scheduler.Jobs;

public class ProcessPendingDiscountsJob : BackgroundService
{
    private readonly IServiceScopeFactory serviceScopeFactory;
    private readonly ILogger<ProcessPendingDiscountsJob> logger;

    public ProcessPendingDiscountsJob(IServiceScopeFactory serviceScopeFactory, ILogger<ProcessPendingDiscountsJob> logger)
    {
        this.serviceScopeFactory = serviceScopeFactory;
        this.logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromHours(1));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            await using var scope = this.serviceScopeFactory.CreateAsyncScope();

            var productService = scope.ServiceProvider.GetRequiredService<IProductService>();
            var result = await productService.EnablePendingDiscounts(stoppingToken);

            this.logger.LogInformation("Finished discount processing job. Enabled: {Enabled}; Disabled: {Disabled}", 
                result.Value.Enabled,
                result.Value.Disabled);
        }
    }
}