// Created on 04/09/2026 19:47 by Laserson

using Lidemia.Core.Enums;
using Lidemia.Core.Models.Base;

namespace Lidemia.Core.Models.Domain;

public class UserModel : AbstractEntity<long>
{
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string PasswordSalt { get; set; } = string.Empty;

    public EntityType Role { get; set; }

    public bool IsBlocked { get; set; }
}