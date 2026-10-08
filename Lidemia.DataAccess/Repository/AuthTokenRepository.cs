// Created on 06/09/2026 14:57 by Laserson

using FluentResults;
using Lidemia.Core.Enums;
using Lidemia.DataAccess.Context;
using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Repository.Abstract;
using LinqToDB;
using LinqToDB.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.DataAccess.Repository;

[ServiceDescriptor<IAuthTokenRepository>(ServiceLifetime.Scoped)]
public class AuthTokenRepository : IAuthTokenRepository
{
    private readonly LidemiaDbContext context;

    public AuthTokenRepository(LidemiaDbContext context)
    {
        this.context = context;
    }

    public Task<AuthTokenEntity?> GetById(long key, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Delete(long key, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<AuthTokenEntity?> GetByAccessToken(string accessToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> Create(long entityId, string accessToken, EntityType entityType, CancellationToken cancellationToken)
    {
        await this.context.AuthTokens
            .ToLinqToDBTable()
            .InsertAsync(() => new AuthTokenEntity
            {
                AccessToken = accessToken,
                EntityId = entityId,
                EntityType = entityType,
                CreateDate = DateTime.UtcNow,
                IsActive = true,
                RowVersion = 1
            }, token: cancellationToken);
        
        return Result.Ok();
    }

    public Task Delete(string accessToken, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}