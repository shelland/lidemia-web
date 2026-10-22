// Created on 13/10/2026 19:40 by Laserson

using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Common.Mapping;
using Lidemia.Core.Models.Domain;
using Lidemia.DataAccess.Repository.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.Common.BusinessLogic.Services.Data;

[ServiceDescriptor<IRegionService>(ServiceLifetime.Scoped)]
public class RegionService : IRegionService
{
    private readonly IRegionRepository regionRepository;

    public RegionService(IRegionRepository regionRepository)
    {
        this.regionRepository = regionRepository;
    }

    public async Task<IEnumerable<RegionModel>> GetRegions(long? countryId, CancellationToken cancellationToken)
    {
        var regions = await this.regionRepository.GetRegions(countryId, cancellationToken);
        return regions.Select(x => x.ToModel());
    }
}