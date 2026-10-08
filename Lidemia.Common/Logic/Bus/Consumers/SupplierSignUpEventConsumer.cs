// Created on 17/09/2026 19:27 by Laserson

using Lidemia.Core.Models.Bus;
using Lidemia.EmailNotifications.Services.Abstract;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Lidemia.Common.Logic.Bus.Consumers;

public class SupplierSignUpEventConsumer : IConsumer<SupplierSignUpBusEventModel>
{
    private readonly ILogger<SupplierSignUpEventConsumer> logger;
    private readonly IEmailNotificationsService emailNotificationsService;

    public SupplierSignUpEventConsumer(ILogger<SupplierSignUpEventConsumer> logger, IEmailNotificationsService emailNotificationsService)
    {
        this.logger = logger;
        this.emailNotificationsService = emailNotificationsService;
    }

    public async Task Consume(ConsumeContext<SupplierSignUpBusEventModel> context)
    {
        await this.emailNotificationsService.SendSupplierSignUpEmail(context.Message.Email, context.Message.Name, context.CancellationToken);
    }
}