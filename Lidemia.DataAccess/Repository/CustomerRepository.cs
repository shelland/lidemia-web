// Created on 01/09/2026 15:46 by Laserson

using FluentResults;
using Lidemia.Core.Enums;
using Lidemia.Core.Models.Service;
using Lidemia.DataAccess.Context;
using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Entities.Meta;
using Lidemia.DataAccess.Extensions;
using Lidemia.DataAccess.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.DataAccess.Repository;

[ServiceDescriptor<ICustomerRepository>(ServiceLifetime.Scoped)]
public class CustomerRepository : ICustomerRepository
{
    private readonly LidemiaDbContext context;

    public CustomerRepository(LidemiaDbContext context)
    {
        this.context = context;
    }

    public Task<CustomerEntity?> GetById(long key, CancellationToken cancellationToken)
    {
        return this.context.Customers
            .AsActive()
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == key, cancellationToken);
    }

    public Task Delete(long key, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<CustomerEntity?> FindCustomerByUserId(long userId, CancellationToken cancellationToken)
    {
        return await this.context
            .Customers
            .AsActive()
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
    }

    public async Task<Result<long>> Create(CreateCustomerModel model, CancellationToken cancellationToken)
    {
        var entity = new CustomerEntity
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Metadata = CustomerMetadata.Default,
            User = new UserEntity
            {
                Email = model.Email,
                Password = model.HashedPassword,
                PasswordSalt = model.PasswordSalt,
                EmailNormalized = model.Email.ToLower(),
                Role = EntityType.Customer
            }
        };

        this.context.Customers.Add(entity);
        await this.context.SaveChangesAsync(cancellationToken);

        return Result.Ok(entity.Id);
    }
}