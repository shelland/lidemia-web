// Created on 03/09/2026 19:57 by Laserson

using Lidemia.Core.Extensions;
using Lidemia.Core.Models.Configuration;
using Lidemia.Core.Models.Dto.Recaptcha;
using Lidemia.ExternalServices.Services.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Scrutor;
using System.Net.Http.Json;

namespace Lidemia.ExternalServices.Services;

[ServiceDescriptor<IRecaptchaService>(ServiceLifetime.Singleton)]
public class RecaptchaService : IRecaptchaService
{
    private readonly IHttpClientFactory httpClientFactory;
    private readonly IOptions<RecaptchaSettingsModel> options;

    public RecaptchaService(IHttpClientFactory httpClientFactory, IOptions<RecaptchaSettingsModel> options)
    {
        this.httpClientFactory = httpClientFactory;
        this.options = options;
    }

    public async Task<bool> Validate(string token, CancellationToken cancellationToken)
    {
        var baseUrl = $"https://recaptchaenterprise.googleapis.com/v1/projects/{this.options.Value.ProjectId}/assessments?key={this.options.Value.ApiKey}";
        var client = this.httpClientFactory.CreateClient(nameof(RecaptchaService));
        var request = new RecaptchaEventRequestDto(Token: token, SiteKey: this.options.Value.SiteKey, UserAgent: string.Empty);

        var content = await client.PostAsJsonAsync(baseUrl, request, cancellationToken);
        var response = (await content.Content.ReadFromJsonAsync<RecaptchaResponseDto>(cancellationToken)).NotNull();

        return response.Score > 0.5;
    }
}