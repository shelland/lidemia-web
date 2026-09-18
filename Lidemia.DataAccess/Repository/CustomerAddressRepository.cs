// Created on 18/09/2026 19:25 by Laserson

using Lidemia.DataAccess.Context;
using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Extensions;
using Lidemia.DataAccess.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.DataAccess.Repository;

[ServiceDescriptor<ICustomerAddressRepository>(ServiceLifetime.Scoped)]
public class CustomerAddressRepository : ICustomerAddressRepository
{
    private readonly LidemiaDbContext context;

    public CustomerAddressRepository(LidemiaDbContext context)
    {
        this.context = context;
    }

    public async Task<IReadOnlyList<AddressEntity>> GetCustomerAddressed(long customerId, CancellationToken cancellationToken)
    {
        return await this.context.CustomerAddresses
            .AsActive()
            .Where(x => x.CustomerId == customerId)
            .Select(x => x.Address)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}