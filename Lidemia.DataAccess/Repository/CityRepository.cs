// Created on 13/10/2026 18:56 by Laserson

using Lidemia.DataAccess.Context;
using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Extensions;
using Lidemia.DataAccess.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.DataAccess.Repository;

[ServiceDescriptor<ICityRepository>(ServiceLifetime.Scoped)]
public class CityRepository : ICityRepository
{
    private readonly LidemiaDbContext context;

    public CityRepository(LidemiaDbContext context)
    {
        this.context = context;
    }

    public Task<CityEntity?> GetById(long key, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Delete(long key, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<CityEntity>> GetCities(long? regionId, CancellationToken cancellationToken)
    {
        var cities = this.context.Cities.AsActive();

        if (regionId != null)
        {
            cities = cities.Where(x => x.RegionId == regionId);
        }

        return await cities.ToListAsync(cancellationToken);
    }
}