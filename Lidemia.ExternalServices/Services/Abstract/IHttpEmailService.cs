// Created on 26/09/2026 19:07 by Laserson

using Lidemia.Core.Models.Dto.External;

namespace Lidemia.ExternalServices.Services.Abstract;

public interface IHttpEmailService : IExternalApiService<HttpEmailRequestDto, HttpEmailResponseDto>
{
}