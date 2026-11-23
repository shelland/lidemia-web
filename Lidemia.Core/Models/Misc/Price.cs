// Created on 18/11/2026 22:44 by Laserson

using Lidemia.Core.Enums;

namespace Lidemia.Core.Models.Misc;

public record Price
(
    decimal Value,
    Currency Currency
);