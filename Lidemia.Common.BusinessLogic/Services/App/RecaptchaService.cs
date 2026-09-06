// Created on 03/09/2026 19:57 by Laserson

using Flurl.Http;
using Lidemia.Common.BusinessLogic.Services.App.Abstract;
using Lidemia.Core.Models.Configuration;
using Lidemia.Core.Models.Dto.Recaptcha;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Scrutor;

namespace Lidemia.Common.BusinessLogic.Services.App;

[ServiceDescriptor<IRecaptchaService>(ServiceLifetime.Singleton)]
public class RecaptchaService : IRecaptchaService
{
    private readonly IOptions<RecaptchaSettingsModel> options;

    public RecaptchaService(IOptions<RecaptchaSettingsModel> options)
    {
        this.options = options;
    }

    public async Task<bool> Validate(string token)
    {
        var baseUrl = $"https://recaptchaenterprise.googleapis.com/v1/projects/{this.options.Value.ProjectId}/assessments?key={this.options.Value.ApiKey}";

        var response = await baseUrl
            .PostJsonAsync(new RecaptchaRequestDto(new RecaptchaEventRequestDto(Token: token, SiteKey: this.options.Value.SiteKey, UserAgent: string.Empty)))
            .ReceiveJson<RecaptchaResponseDto>();

        return response.Score > 0.5;
    }
}