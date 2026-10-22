// Created on 03/09/2026 18:49 by Laserson

using Lidemia.DataAccess.Context;
using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Repository.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.DataAccess.Repository;

[ServiceDescriptor<IPasswordResetRepository>(ServiceLifetime.Scoped)]
public class PasswordResetRepository : IPasswordResetRepository
{
    private readonly LidemiaDbContext context;

    public PasswordResetRepository(LidemiaDbContext context)
    {
        this.context = context;
    }

    public Task<PasswordResetEntity?> GetById(long key, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Delete(long key, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}