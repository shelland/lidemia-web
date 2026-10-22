// Created on 13/10/2026 19:36 by Laserson

using Lidemia.Core.Models.Domain;

namespace Lidemia.Common.BusinessLogic.Services.Data.Abstract;

public interface IRegionService
{
    Task<IEnumerable<RegionModel>> GetRegions(long? countryId, CancellationToken cancellationToken);
}