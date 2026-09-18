// Created on 17/09/2026 19:16 by Laserson

namespace Lidemia.EmailNotifications.Services.Abstract;

public interface IEmailNotificationsService
{
    Task SendSupplierSignUpEmail(string email, string name, CancellationToken cancellationToken);
}