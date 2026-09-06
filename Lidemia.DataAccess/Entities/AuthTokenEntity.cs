// Created on 11/12/2021 21:48 by Andrey Laserson

using Lidemia.Core.Enums;
using Lidemia.DataAccess.Abstract;

namespace Lidemia.DataAccess.Entities;

public class AuthTokenEntity : IDbEntity
{
    public string AccessToken { get; set; } = string.Empty;

    public string? RefreshToken { get; set; }

    public DateTimeOffset? ExpirationDate { get; set; }

    public long EntityId { get; set; }

    public EntityType EntityType { get; set; }

    public int RowVersion { get; set; }

    public bool IsActive { get; set; }

    public DateTimeOffset CreateDate { get; set; }

    public DateTimeOffset? UpdateDate { get; set; }
}