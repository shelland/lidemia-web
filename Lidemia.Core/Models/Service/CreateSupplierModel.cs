// Created on 06/09/2026 14:06 by Laserson

using Lidemia.Core.Enums;

namespace Lidemia.Core.Models.Service;

public record CreateSupplierModel
(
    string Inn,
    string Ogrn,
    string Name,
    string Email,
    string HashedPassword,
    string PasswordSalt,
    OrganizationType OrganizationType
);