// Created on 02/09/2026 20:15 by Laserson

using Lidemia.Core.Enums;
using Lidemia.Core.Models.Base;
using Lidemia.DataAccess.Abstract;

namespace Lidemia.DataAccess.Entities;

public class PasswordResetEntity : IHasId<Guid>, IDbEntity
{
    public Guid Id { get; set; }

    public long UserId { get; set; }

    public UserEntity User { get; set; } = null!;

    public EntityType EntityType { get; set; }

    public string Code { get; set; } = string.Empty;

    public int RowVersion { get; set; }

    public bool IsActive { get; set; }

    public DateTimeOffset CreateDate { get; set; }

    public DateTimeOffset? UpdateDate { get; set; }
}