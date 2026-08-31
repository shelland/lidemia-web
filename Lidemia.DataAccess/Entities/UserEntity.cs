// Created on 18/11/2021 22:54 by Andrey Laserson

using Lidemia.Core.Enums;
using Lidemia.DataAccess.Abstract;

namespace Lidemia.DataAccess.Entities;

public class UserEntity : IDbEntity, IHasId<long>
{
    public long Id { get; set; }

    public string Email { get; set; } = null!;

    public string EmailNormalized { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string PasswordSalt { get; set; } = string.Empty;

    public bool IsBlocked { get; set; }

    public EntityType Role { get; set; }

    public int RowVersion { get; set; }

    public bool IsActive { get; set; }

    public DateTimeOffset CreateDate { get; set; }

    public DateTimeOffset? UpdateDate { get; set; }
}