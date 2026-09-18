// Created on 17/09/2026 19:27 by Laserson

using Lidemia.Core.Models.Bus;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Lidemia.Common.Logic.Bus.Consumers;

public class SupplierSignUpEventConsumer : IConsumer<SupplierSignUpBusEventModel>
{
    private readonly ILogger<SupplierSignUpEventConsumer> logger;

    public SupplierSignUpEventConsumer(ILogger<SupplierSignUpEventConsumer> logger)
    {
        this.logger = logger;
    }

    public Task Consume(ConsumeContext<SupplierSignUpBusEventModel> context)
    {
        return Task.CompletedTask;
    }
}