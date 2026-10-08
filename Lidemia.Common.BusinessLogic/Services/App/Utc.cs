// Created on 01/10/2026 18:52 by Laserson

using Lidemia.Common.BusinessLogic.Services.App.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.Common.BusinessLogic.Services.App;

[ServiceDescriptor<IUtc>(ServiceLifetime.Transient)]
public class Utc : IUtc
{
    public DateTimeOffset Now { get; } = DateTimeOffset.UtcNow;
}