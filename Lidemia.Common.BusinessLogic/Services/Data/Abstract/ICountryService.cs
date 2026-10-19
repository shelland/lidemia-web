// Created on 13/10/2026 19:36 by Laserson

using Lidemia.Core.Models.Domain;

namespace Lidemia.Common.BusinessLogic.Services.Data.Abstract;

public interface ICountryService
{
    Task<IEnumerable<CountryModel>> GetCountries(CancellationToken cancellationToken);
}