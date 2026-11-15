// Created on 30/10/2026 20:37 by Laserson

using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Core;
using Lidemia.DataAccess.Repository.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Lidemia.Scheduler.Jobs;

public class RecalculateProductRatingsJob : BackgroundService
{
    private readonly IServiceScopeFactory serviceScopeFactory;
    private readonly ILogger<RecalculateProductRatingsJob> logger;

    public RecalculateProductRatingsJob(IServiceScopeFactory serviceScopeFactory, ILogger<RecalculateProductRatingsJob> logger)
    {
        this.serviceScopeFactory = serviceScopeFactory;
        this.logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(Constants.Delays.OneMinute);

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            await using var scope = this.serviceScopeFactory.CreateAsyncScope();
            var reviewRepository = scope.ServiceProvider.GetRequiredService<IProductReviewStatsRepository>();

            await reviewRepository.Recalculate(stoppingToken);
        }
    }
}