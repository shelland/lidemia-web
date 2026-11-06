// Created on 04/11/2026 13:53 by Laserson

using Lidemia.Core.Models.Bus;
using MassTransit;

namespace Lidemia.Common.Logic.Bus.Consumers;

public class OrderCreatedEventConsumer : IConsumer<OrderCreatedBusEventModel>
{
    public Task Consume(ConsumeContext<OrderCreatedBusEventModel> context)
    {
        throw new NotImplementedException();
    }
}