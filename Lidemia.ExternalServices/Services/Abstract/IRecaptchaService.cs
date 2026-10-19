// Created on 03/09/2026 19:57 by Laserson

using Lidemia.Core.Models.Dto.Recaptcha;

namespace Lidemia.ExternalServices.Services.Abstract;

public interface IRecaptchaService : IExternalApiService<RecaptchaEventRequestDto, RecaptchaResponseDto>
{
    Task<bool> Validate(string token, CancellationToken cancellationToken);
}