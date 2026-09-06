// Created on 06/09/2026 15:21 by Laserson

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SolrNet;

namespace Lidemia.Search;

public static class SearchModule
{
    public static IServiceCollection AddSearchModule(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection("Integrations:Solr");

        var credentials = System.Text.Encoding.ASCII.GetBytes($"{section.GetValue<string>("Name")}:{section.GetValue<string>("Password")}");
        var credentialsBase64 = Convert.ToBase64String(credentials);

        var coreName = section.GetValue<string>("CoreName");

        services.AddSolrNet($"{section.GetValue<string>("Url")}/solr/{coreName}", options =>
        {
            options.HttpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentialsBase64);
        });

        services.Scan(x => x.FromAssemblyOf<ISearchModule>().AddClasses().UsingAttributes());

        return services;
    }
}

internal interface ISearchModule;