// Created on 01/09/2026 15:44 by Laserson

using FluentResults;
using Lidemia.DataAccess.Context;
using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Models;
using Lidemia.DataAccess.Repository.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.DataAccess.Repository;

[ServiceDescriptor<ISupplierRepository>(ServiceLifetime.Scoped)]
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

    public async Task<Result<long>> Create(CreateSupplierModel model, CancellationToken cancellationToken)
    {
        var entity = new SupplierEntity
        {
            User = new UserEntity
            {
                Email = model.Email,
                Password = model.HashedPassword,
                PasswordSalt = model.PasswordSalt
            }
        };

        this.context.Suppliers.Add(entity);
        await this.context.SaveChangesAsync(cancellationToken);

        return Result.Ok(entity.Id);
    }
}