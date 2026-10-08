// Created on 06/09/2026 15:21 by Laserson

using Lidemia.Core.Models.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenSearch.Client;
using OpenSearch.Net;

namespace Lidemia.Search;

public static class SearchModule
{
    public static IServiceCollection AddSearchModule(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection("LocalServices:OpenSearch");

        services.Configure<OpenSearchSettings>(section);

        var settings = section.Get<OpenSearchSettings>() ?? new OpenSearchSettings();

        var connectionSettings = new ConnectionSettings(new Uri(settings.Url))
            .DefaultIndex(settings.IndexName)
            .DefaultFieldNameInferrer(InferFieldName);

        if (!string.IsNullOrWhiteSpace(settings.Name) && !string.IsNullOrWhiteSpace(settings.Password))
        {
            connectionSettings = connectionSettings.BasicAuthentication(settings.Name, settings.Password);
        }

        if (settings.AllowInvalidCertificates)
        {
            connectionSettings = connectionSettings.ServerCertificateValidationCallback(CertificateValidations.AllowAll);
        }

        services.AddSingleton<IOpenSearchClient>(new OpenSearchClient(connectionSettings));
        services.Scan(x => x.FromAssemblyOf<ISearchModule>().AddClasses().UsingAttributes());

        return services;
    }

    private static string InferFieldName(string propertyName)
    {
        if (string.IsNullOrEmpty(propertyName))
        {
            return propertyName;
        }

        return char.ToLowerInvariant(propertyName[0]) + propertyName[1..];
    }
}

internal interface ISearchModule;