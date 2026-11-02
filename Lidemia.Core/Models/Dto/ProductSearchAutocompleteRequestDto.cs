// Created on 02/11/2026 20:02 by Laserson

namespace Lidemia.Core.Models.Dto;

public record ProductSearchAutocompleteRequestDto
(
    string Query,
    int Limit = 8
);