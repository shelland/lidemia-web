// Created on 13/10/2026 18:56 by Laserson

using Lidemia.DataAccess.Context;
using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Extensions;
using Lidemia.DataAccess.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.DataAccess.Repository;

[ServiceDescriptor<ICountryRepository>(ServiceLifetime.Scoped)]
public class CountryRepository : ICountryRepository
{
    private readonly LidemiaDbContext context;

    public CountryRepository(LidemiaDbContext context)
    {
        this.context = context;
    }

    public Task<CountryEntity?> GetById(Guid key, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Delete(Guid key, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<CountryEntity>> GetCountries(CancellationToken cancellationToken)
    {
        return await this.context.Countries.AsActive().ToListAsync(cancellationToken);
    }
}