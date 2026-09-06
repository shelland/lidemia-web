// Created on 04/09/2026 19:28 by Laserson

using Lidemia.Core.Enums;

namespace Lidemia.Core.Models.Dto;

public record SignInResult<TEntity>
(
    LoginResultStatus Status,
    TEntity? Entity
);