// Created on 07/09/2026 21:59 by Laserson

using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using System.Diagnostics.Metrics;

namespace Lidemia.Common.Extensions;

[ServiceDescriptor<SupplierMetrics>(ServiceLifetime.Singleton)]
public class SupplierMetrics
{
    private readonly Counter<int> totalSuppliers;

    public SupplierMetrics(IMeterFactory factory)
    {
        var meter = factory.CreateApp();
        this.totalSuppliers = meter.CreateCounter<int>("TotalSuppliers");
    }

    public void OnNewSupplier()
    {
        this.totalSuppliers.Add(1);
    }
}