// Created on 04/09/2026 19:47 by Laserson

using Lidemia.Core.Enums;
using Lidemia.Core.Models.Domain;

namespace Lidemia.Common.BusinessLogic.Services.Data.Abstract;

public interface IUserService
{
    Task<UserModel?> FindUser(string email, string password, EntityType role, CancellationToken cancellationToken);
}