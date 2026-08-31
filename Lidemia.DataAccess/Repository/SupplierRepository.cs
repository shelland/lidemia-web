// Created on 01/09/2026 15:44 by Laserson

using Lidemia.DataAccess.Context;
using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Repository.Abstract;

namespace Lidemia.DataAccess.Repository;

public class SupplierRepository : ISupplierRepository
{
    private readonly LidemiaDbContext context;

    public SupplierRepository(LidemiaDbContext context)
    {
        this.context = context;
    }

    public Task<SupplierEntity?> GetById(long key, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Delete(long key, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}