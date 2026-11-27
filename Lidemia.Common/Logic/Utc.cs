// Created on 01/10/2026 18:52 by Laserson

using Lidemia.Common.Logic.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.Common.Logic;

[ServiceDescriptor<IUtc>(ServiceLifetime.Transient)]
public class Utc : IUtc
{
    public DateTimeOffset Now => DateTimeOffset.UtcNow;
}