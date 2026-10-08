// Created on 23/09/2026 23:39 by Laserson

namespace Lidemia.Core.Models.Configuration;

public class IntegrationsSettingsModel
{
    public RecaptchaSettingsModel Recaptcha { get; set; } = null!;

    public SmtpEmailSettings SmtpEmail { get; set; } = null!;

    public HttpEmailSettings HttpEmail { get; set; } = null!;
}

public class RecaptchaSettingsModel
{
    public string ApiKey { get; set; } = null!;

    public string ProjectId { get; set; } = null!;

    public string SiteKey { get; set; } = null!;

    public string SecretKey { get; set; } = null!;
}

public class SmtpEmailSettings
{
    public string Host { get; set; } = string.Empty;

    public ushort Port { get; set; }

    public string Login { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}

public class HttpEmailSettings
{
    public string Url { get; set; } = string.Empty;

    public string ApiKey { get; set; } = string.Empty;
}