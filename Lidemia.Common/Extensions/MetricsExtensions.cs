// Created on 04/03/2026 23:01 by Laserson

using Lidemia.Core;
using System.Diagnostics.Metrics;

namespace Lidemia.Common.Extensions;

public static class MetricsExtensions
{
    public static Meter CreateApp(this IMeterFactory meterFactory)
    {
        return meterFactory.Create(Constants.AppName);
    }
}