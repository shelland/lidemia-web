// Created on 02/09/2026 20:04 by Laserson

namespace Lidemia.Core.Models.Dto;

public record SupplierSignUpRequestDto
(
    string Email,
    string Name,
    string FullName,
    string Password
);