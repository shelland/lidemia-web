// Created on 02/09/2026 20:04 by Laserson

using Lidemia.Core.Enums;

namespace Lidemia.Core.Models.Dto;

public record SupplierSignUpRequestDto
(
    string Email,
    string Name,
    string Password,
    string Inn,
    string Ogrn,
    string Phone,
    OrganizationType Type
);