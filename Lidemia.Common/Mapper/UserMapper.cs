// Created on 26/10/2026 19:56 by Laserson

using Lidemia.Common.Mapper.Abstract;
using Lidemia.Core.Models.Domain;
using Lidemia.DataAccess.Entities;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.Common.Mapper;

public interface IUserMapper : IMapper<UserEntity, UserModel>;

[ServiceDescriptor<IUserMapper>(ServiceLifetime.Scoped)]
public class UserMapper : IUserMapper
{
    public UserModel Map(UserEntity source)
    {
        return new UserModel
        {
            Id = source.Id,
            Email = source.Email,
            Password = source.Password,
            PasswordSalt = source.PasswordSalt,
            Role = source.Role,
            IsBlocked = source.IsBlocked,
            CreateDate = source.CreateDate,
            UpdateDate = source.UpdateDate
        };
    }
}