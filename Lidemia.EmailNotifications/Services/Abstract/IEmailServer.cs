// Created on 24/09/2026 00:01 by Laserson

namespace Lidemia.EmailNotifications.Services.Abstract;

public interface IEmailServer
{
    string Host { get; set; }

    ushort Port { get; set; }

    string Login { get; set; }

    string Password { get; set; }
}