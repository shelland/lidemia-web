// Created on 13/10/2026 19:37 by Laserson

using Lidemia.Core.Models.Domain;

namespace Lidemia.Common.BusinessLogic.Services.Data.Abstract;

public interface ICityService
{
    Task<IEnumerable<CityModel>> GetCities(Guid? regionId, CancellationToken cancellationToken);
}