// Created on 20/02/2021 13:33 by Andrey Laserson

using Lidemia.Core.Models.Dto.External;

namespace Lidemia.ExternalServices.Services.Abstract;

public interface IGeoIpApiService : IExternalApiService<GeoIpApiServiceRequestModel, GeoIpApiServiceResponseModel>;