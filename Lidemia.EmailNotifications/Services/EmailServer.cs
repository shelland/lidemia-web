// Created on 24/09/2026 00:02 by Laserson

using Lidemia.Core.Models.Configuration;
using Lidemia.EmailNotifications.Services.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Scrutor;

namespace Lidemia.EmailNotifications.Services;

[ServiceDescriptor<IEmailServer>(ServiceLifetime.Singleton)]
public class EmailServer : IEmailServer
{
    public EmailServer(IOptions<IntegrationsSettingsModel> options)
    {
        Host = options.Value.SmtpEmail.Host;
        Port = options.Value.SmtpEmail.Port;
        Login = options.Value.SmtpEmail.Login;
        Password = options.Value.SmtpEmail.Password;
    }

    public string Host { get; set; }

    public ushort Port { get; set; }

    public string Login { get; set; }

    public string Password { get; set; }
}