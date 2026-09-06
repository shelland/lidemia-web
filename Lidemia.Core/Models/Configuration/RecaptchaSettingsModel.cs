// Created on 06/10/2020 8:42 by Andrey Laserson

namespace Lidemia.Core.Models.Configuration;

public class RecaptchaSettingsModel
{
    public string ApiKey { get; set; } = null!;

    public string ProjectId { get; set; } = null!;

    public string SiteKey { get; set; } = null!;

    public string SecretKey { get; set; } = null!;
}