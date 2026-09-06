// Created on 06/09/2026 14:57 by Laserson

using FluentResults;
using Lidemia.Core.Enums;
using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Repository.Abstract;

namespace Lidemia.DataAccess.Repository;

public class AuthTokenRepository : IAuthTokenRepository
{
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

    public Task<Result> Create(long entityId, string accessToken, EntityType entityType, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Delete(string accessToken, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}