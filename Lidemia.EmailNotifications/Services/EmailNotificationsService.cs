// Created on 17/09/2026 19:16 by Laserson

using Lidemia.EmailNotifications.Services.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.EmailNotifications.Services;

[ServiceDescriptor<IEmailNotificationsService>(ServiceLifetime.Transient)]
public class EmailNotificationsService : IEmailNotificationsService
{
    public Task SendSupplierSignUpEmail(string email, string name, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}