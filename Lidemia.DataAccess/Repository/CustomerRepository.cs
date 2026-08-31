// Created on 01/09/2026 15:46 by Laserson

using Lidemia.DataAccess.Context;
using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Repository.Abstract;

namespace Lidemia.DataAccess.Repository;

public class CustomerRepository : ICustomerRepository
{
    private readonly LidemiaDbContext context;

    public CustomerRepository(LidemiaDbContext context)
    {
        this.context = context;
    }

    public Task<CustomerEntity?> GetById(long key, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Delete(long key, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}