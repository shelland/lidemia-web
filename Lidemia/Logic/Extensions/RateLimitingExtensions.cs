// Created on 30/10/2026 20:28 by Laserson

using Lidemia.Core;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

namespace Lidemia.Logic.Extensions;

public static class RateLimitingExtensions
{
    public static IServiceCollection AddRateLimiting(this IServiceCollection service)
    {
        service.AddRateLimiter(opts =>
        {
            opts.AddFixedWindowLimiter(Constants.RateLimitingPolicies.ProductReviewsPolicy, cfg =>
            {
                cfg.Window = TimeSpan.FromSeconds(10);
                cfg.PermitLimit = 2;
                cfg.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            });
        });

        return service;
    }
}