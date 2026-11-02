// Created on 02/11/2026 20:03 by Laserson

using Lidemia.Core.Models.Misc;

namespace Lidemia.Core.Models.Dto;

public record ProductSearchAutocompleteResponseDto
(
    IEnumerable<ProductMainInfoModel> Results
);