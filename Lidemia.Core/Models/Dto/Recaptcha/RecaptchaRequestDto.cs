// Created on 03/09/2026 20:00 by Laserson

namespace Lidemia.Core.Models.Dto.Recaptcha;

public record RecaptchaRequestDto(RecaptchaEventRequestDto Event);

public record RecaptchaEventRequestDto
(
    string Token,
    string SiteKey,
    string UserAgent
);