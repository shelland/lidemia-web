// Created on 18/07/2023 20:45 by shell

namespace Lidemia.Core.Models.Configuration;

public class AppIdSettings
{
    public string AppName { get; set; } = string.Empty;

    public string Module { get; set; } = string.Empty;

    public string Environment { get; set; } = string.Empty;

    public string SiteVersion { get; set; } = string.Empty;

    public override string ToString() => $"{AppName}:{Module}:{Environment}";
}