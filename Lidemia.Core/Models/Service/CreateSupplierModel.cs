// Created on 06/09/2026 14:06 by Laserson

namespace Lidemia.DataAccess.Models;

public record CreateSupplierModel
(
    string Email,
    string HashedPassword,
    string PasswordSalt
);