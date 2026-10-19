// Created on 07/09/2026 21:59 by Laserson

using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using System.Diagnostics.Metrics;

namespace Lidemia.Common.Extensions;

[ServiceDescriptor<CustomerMetrics>(ServiceLifetime.Singleton)]
public class CustomerMetrics
{
    private readonly Counter<int> totalCustomers;

    public CustomerMetrics(IMeterFactory factory)
    {
        var meter = factory.CreateApp();
        this.totalCustomers = meter.CreateCounter<int>("TotalCustomers");
    }

    public void OnNewCustomer()
    {
        this.totalCustomers.Add(1);
    }
}