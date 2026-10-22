// Created on 13/10/2026 18:54 by Laserson

using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Repository.Abstract.Base;

namespace Lidemia.DataAccess.Repository.Abstract;

public interface IRegionRepository : IRepository<RegionEntity, long>
{
    Task<IEnumerable<RegionEntity>> GetRegions(long? countryId, CancellationToken cancellationToken);
}