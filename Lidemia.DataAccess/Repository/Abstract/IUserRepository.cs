// Created on 04/09/2026 19:41 by Laserson

using Lidemia.Core.Enums;
using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Repository.Abstract.Base;

namespace Lidemia.DataAccess.Repository.Abstract;

public interface IUserRepository : IRepository<UserEntity, long>
{
    Task<UserEntity?> FindUser(string email, EntityType role, CancellationToken cancellationToken);

    Task<bool> IsEmailExists(string email, CancellationToken cancellationToken);
}