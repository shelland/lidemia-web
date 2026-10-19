// Created on 13/10/2026 19:40 by Laserson

using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Common.Mapping;
using Lidemia.Core.Models.Domain;
using Lidemia.DataAccess.Repository.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.Common.BusinessLogic.Services.Data;

[ServiceDescriptor<ICityService>(ServiceLifetime.Scoped)]
public class CityService : ICityService
{
    private readonly ICityRepository cityRepository;

    public CityService(ICityRepository cityRepository)
    {
        this.cityRepository = cityRepository;
    }

    public async Task<IEnumerable<CityModel>> GetCities(Guid? regionId, CancellationToken cancellationToken)
    {
        var cities = await this.cityRepository.GetCities(regionId, cancellationToken);
        return cities.Select(x => x.ToModel());
    }
}