// Created on 03/09/2026 19:57 by Laserson

namespace Lidemia.ExternalServices.Services.Abstract;

public interface IRecaptchaService
{
    Task<bool> Validate(string token, CancellationToken cancellationToken);
}