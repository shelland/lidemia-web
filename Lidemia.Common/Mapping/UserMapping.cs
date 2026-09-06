// Created on 04/09/2026 19:49 by Laserson

using Lidemia.Core.Models.Domain;
using Lidemia.DataAccess.Entities;

namespace Lidemia.Common.Mapping;

public static class UserMapping
{
    public static UserModel ToModel(this UserEntity entity)
    {
        return new UserModel
        {
            Id = entity.Id,
            Email = entity.Email,
            Password = entity.Password,
            PasswordSalt = entity.PasswordSalt,
            Role = entity.Role,
            IsBlocked = entity.IsBlocked,
            CreateDate = entity.CreateDate,
            UpdateDate = entity.UpdateDate
        };
    }
}