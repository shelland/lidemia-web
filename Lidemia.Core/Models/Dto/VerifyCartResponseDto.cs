// Created on 09/11/2026 13:03 by Laserson

using Lidemia.Core.Models.Misc;

namespace Lidemia.Core.Models.Dto;

public record VerifyCartResponseDto
(
    decimal NewTotal,
    IEnumerable<LightProductModel> ChangedProducts,
    IEnumerable<LightProductModel> MissingProducts
);