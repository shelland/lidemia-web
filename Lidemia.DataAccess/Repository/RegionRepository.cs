// Created on 13/10/2026 18:56 by Laserson

using Lidemia.DataAccess.Context;
using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Extensions;
using Lidemia.DataAccess.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.DataAccess.Repository;

[ServiceDescriptor<IRegionRepository>(ServiceLifetime.Scoped)]
public class RegionRepository : IRegionRepository
{
    private readonly LidemiaDbContext context;

    public RegionRepository(LidemiaDbContext context)
    {
        this.context = context;
    }

    public Task<RegionEntity?> GetById(long key, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Delete(long key, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<RegionEntity>> GetRegions(long? countryId, CancellationToken cancellationToken)
    {
        var regions = this.context.Regions.AsActive();

        if (countryId != null)
        {
            regions = regions.Where(x => x.CountryId == countryId);
        }

        return await regions.ToListAsync(cancellationToken);
    }
}