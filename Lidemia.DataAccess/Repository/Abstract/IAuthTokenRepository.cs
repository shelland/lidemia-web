// Created on 11/12/2021 21:49 by Andrey Laserson

using FluentResults;
using Lidemia.Core.Enums;
using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Repository.Abstract.Base;

namespace Lidemia.DataAccess.Repository.Abstract;

public interface IAuthTokenRepository : IRepository<AuthTokenEntity, long>
{
    Task<AuthTokenEntity?> GetByAccessToken(string accessToken);

    Task<Result> Create(long entityId, string accessToken, EntityType entityType, CancellationToken cancellationToken);

    Task Delete(string accessToken, CancellationToken cancellationToken);
}