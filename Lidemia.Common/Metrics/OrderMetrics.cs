// Created on 07/09/2026 21:54 by Laserson

using Lidemia.Common.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using System.Diagnostics.Metrics;

namespace Lidemia.Common.Metrics;

[ServiceDescriptor<OrderMetrics>(ServiceLifetime.Singleton)]
public class OrderMetrics
{
    private readonly Counter<int> totalOrdersCounter;

    public OrderMetrics(IMeterFactory factory)
    {
        var meter = factory.CreateApp();
        this.totalOrdersCounter = meter.CreateCounter<int>("TotalOrders");
    }

    public void OnNewOrder()
    {
        this.totalOrdersCounter.Add(1);
    }
}