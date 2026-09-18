// Created on 06/09/2026 14:06 by Laserson

namespace Lidemia.Core.Models.Service;

public record CreateCustomerModel
(
    string Email,
    string HashedPassword,
    string PasswordSalt,
    string FirstName,
    string LastName
);