// Created on 26/09/2026 19:17 by Laserson

using Lidemia.Core.Extensions;
using Lidemia.Core.Models.Configuration;
using Lidemia.Core.Models.Dto.External;
using Lidemia.Core.Models.Service;
using Lidemia.ExternalServices.Services.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Scrutor;
using System.Net.Http.Json;

namespace Lidemia.ExternalServices.Services;

[ServiceDescriptor<IHttpEmailService>(ServiceLifetime.Singleton)]
public class HttpEmailService : IHttpEmailService
{
    private readonly IHttpClientFactory httpClientFactory;
    private readonly IOptions<IntegrationsSettingsModel> options;
    private readonly ILogger<HttpEmailService> logger;

    public HttpEmailService(IHttpClientFactory httpClientFactory, IOptions<IntegrationsSettingsModel> options, ILogger<HttpEmailService> logger)
    {
        this.httpClientFactory = httpClientFactory;
        this.options = options;
        this.logger = logger;
    }

    public string BaseApiUrl => this.options.Value.HttpEmail.Url;

    public async Task<ResultInfo<HttpEmailResponseDto>> MakeRequest(HttpEmailRequestDto request, CancellationToken cancellationToken)
    {
        try
        {
            var client = this.httpClientFactory.CreateClient(nameof(HttpEmailService));
            client.DefaultRequestHeaders.Add("Authorization", this.options.Value.HttpEmail.ApiKey);
            var result = await (await client.PostAsJsonAsync(this.BaseApiUrl, request, cancellationToken: cancellationToken)).Content
                .ReadFromJsonAsync<HttpEmailResponseDto>(cancellationToken: cancellationToken);

            return new ResultInfo<HttpEmailResponseDto>(result.NotNull());
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, ex.Message);
            throw;
        }
    }
}