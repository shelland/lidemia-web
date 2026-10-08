// Created on 24/09/2023 19:04 by shell

using Lidemia.Core.Models.Service;

namespace Lidemia.ExternalServices.Services.Abstract;

public interface IExternalApiService<in TRequest, TResponse>
{
    public string BaseApiUrl { get; }

    Task<ResultInfo<TResponse>> MakeRequest(TRequest request, CancellationToken cancellationToken);
}