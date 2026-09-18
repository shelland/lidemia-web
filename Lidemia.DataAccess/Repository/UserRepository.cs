// Created on 04/09/2026 19:41 by Laserson

using Lidemia.Core.Enums;
using Lidemia.DataAccess.Context;
using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Extensions;
using Lidemia.DataAccess.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.DataAccess.Repository;

[ServiceDescriptor<IUserRepository>(ServiceLifetime.Scoped)]
public class UserRepository : IUserRepository
{
    private readonly LidemiaDbContext context;

    public UserRepository(LidemiaDbContext context)
    {
        this.context = context;
    }

    public async Task<UserEntity?> FindUser(string email, EntityType role, CancellationToken cancellationToken)
    {
        var emailNormalized = email.ToLower();

        var user = await this.context.Users
            .AsActive()
            .FirstOrDefaultAsync(x =>
                x.Email.Equals(emailNormalized) &&
                x.Role == role, cancellationToken);

        return user;
    }

    public async Task<bool> IsEmailExists(string email, CancellationToken cancellationToken)
    {
        var normalizedEmail = email.ToLower();
        return await this.context.Users.CountAsync(x => x.EmailNormalized.Equals(normalizedEmail), cancellationToken) > 0;
    }

    public Task<UserEntity?> GetById(long key, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Delete(long key, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}