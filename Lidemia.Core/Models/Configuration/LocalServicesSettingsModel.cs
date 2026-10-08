// Created on 23/09/2026 23:35 by Laserson

namespace Lidemia.Core.Models.Configuration;

public class LocalServicesSettingsModel
{
    public OpenSearchSettings OpenSearch { get; set; } = null!;

    public SeqSettings Seq { get; set; } = null!;

    public RabbitSettings Rabbit { get; set; } = null!;
}

public class OpenSearchSettings
{
    public string Url { get; set; } = "http://127.0.0.1:9200";

    public string Name { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string IndexName { get; set; } = "lidemia-products";

    public bool AllowInvalidCertificates { get; set; }
}

public class SeqSettings
{
}

public class RabbitSettings
{
}