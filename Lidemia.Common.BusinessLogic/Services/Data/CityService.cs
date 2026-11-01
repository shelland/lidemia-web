// Created on 13/10/2026 19:40 by Laserson

using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Common.Mapper;
using Lidemia.Core.Models.Domain;
using Lidemia.DataAccess.Repository.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.Common.BusinessLogic.Services.Data;

[ServiceDescriptor<ICityService>(ServiceLifetime.Scoped)]
public class CityService : ICityService
{
    private readonly ICityRepository cityRepository;
    private readonly ICityMapper cityMapper;

    public CityService(ICityRepository cityRepository, ICityMapper cityMapper)
    {
        this.cityRepository = cityRepository;
        this.cityMapper = cityMapper;
    }

    public async Task<IEnumerable<CityModel>> GetCities(long? regionId, CancellationToken cancellationToken)
    {
        var cities = await this.cityRepository.GetCities(regionId, cancellationToken);
        return cities.Select(this.cityMapper.Map);
    }
}