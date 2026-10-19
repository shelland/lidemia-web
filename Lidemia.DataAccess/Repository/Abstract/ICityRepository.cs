// Created on 13/10/2026 18:54 by Laserson

using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Repository.Abstract.Base;

namespace Lidemia.DataAccess.Repository.Abstract;

public interface ICityRepository : IRepository<CityEntity, Guid>
{
    Task<IEnumerable<CityEntity>> GetCities(Guid? regionId, CancellationToken cancellationToken);
}