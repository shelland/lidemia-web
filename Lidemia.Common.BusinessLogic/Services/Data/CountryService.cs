// Created on 13/10/2026 19:40 by Laserson

using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Common.Mapping;
using Lidemia.Core.Models.Domain;
using Lidemia.DataAccess.Repository.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.Common.BusinessLogic.Services.Data;

[ServiceDescriptor<ICountryService>(ServiceLifetime.Scoped)]
public class CountryService : ICountryService
{
    private readonly ICountryRepository countryRepository;

    public CountryService(ICountryRepository countryRepository)
    {
        this.countryRepository = countryRepository;
    }

    public async Task<IEnumerable<CountryModel>> GetCountries(CancellationToken cancellationToken)
    {
        var countries = await this.countryRepository.GetCountries(cancellationToken);
        return countries.Select(x => x.ToModel());
    }
}