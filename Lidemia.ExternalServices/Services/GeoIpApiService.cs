// Created on 20/02/2021 13:33 by Andrey Laserson

using Lidemia.Core.Extensions;
using Lidemia.Core.Models.Dto.External;
using Lidemia.Core.Models.Service;
using Lidemia.ExternalServices.Services.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Scrutor;
using System.Net.Http.Json;

namespace Lidemia.ExternalServices.Services;

[ServiceDescriptor<IGeoIpApiService>(ServiceLifetime.Transient)]
public class GeoIpApiService : IGeoIpApiService
{
    private readonly IHttpClientFactory httpClientFactory;
    private readonly ILogger<GeoIpApiService> logger;

    public GeoIpApiService(IHttpClientFactory httpClientFactory, ILogger<GeoIpApiService> logger)
    {
        this.httpClientFactory = httpClientFactory;
        this.logger = logger;
    }

    public string BaseApiUrl => "http://ip-api.com/json/";

    public async Task<ResultInfo<GeoIpApiServiceResponseModel>> MakeRequest(GeoIpApiServiceRequestModel request, CancellationToken cancellationToken)
    {
        try
        {
            using var client = this.httpClientFactory.CreateClient(nameof(GeoIpApiService));
            var finalUrl = $"{BaseApiUrl}{request.Ip}";

            if (!string.IsNullOrEmpty(request.Locale))
            {
                finalUrl += $"?lang={request.Locale}";
            }

            var result =
                (await (await client.GetAsync(finalUrl, cancellationToken)).Content.ReadFromJsonAsync<GeoIpApiServiceResponseModel>(
                    cancellationToken: cancellationToken)).NotNull();

            this.logger.LogInformation("Location fetched for {Ip} as {Lat}/{Lng} ({City})", request.Ip, result.Lat, result.Lon, result.City);
            return new ResultInfo<GeoIpApiServiceResponseModel>(result);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, ex.Message);
            throw;
        }
    }
}