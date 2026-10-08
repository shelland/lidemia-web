// Created on 26/09/2026 18:57 by Laserson

using Lidemia.EmailNotifications.Services.Abstract;

namespace Lidemia.EmailNotifications.Services;

public class HttpEmailNotificationService : IEmailNotificationsService
{
    public Task SendSupplierSignUpEmail(string email, string name, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}