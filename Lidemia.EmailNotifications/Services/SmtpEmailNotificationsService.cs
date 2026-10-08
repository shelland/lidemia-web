// Created on 17/09/2026 19:16 by Laserson

using Lidemia.EmailNotifications.Services.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Scrutor;

namespace Lidemia.EmailNotifications.Services;

[ServiceDescriptor<IEmailNotificationsService>(ServiceLifetime.Transient)]
public class SmtpEmailNotificationsService : IEmailNotificationsService
{
    private readonly IEmailServer emailServer;
    private readonly ILogger<SmtpEmailNotificationsService> logger;

    public SmtpEmailNotificationsService(IEmailServer emailServer, ILogger<SmtpEmailNotificationsService> logger)
    {
        this.emailServer = emailServer;
        this.logger = logger;
    }

    public Task SendSupplierSignUpEmail(string email, string name, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Sending welcome email to supplier {Email}", email);
        return Task.CompletedTask;
    }
}