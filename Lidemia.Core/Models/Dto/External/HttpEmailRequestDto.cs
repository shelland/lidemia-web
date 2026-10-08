// Created on 26/09/2026 19:08 by Laserson

namespace Lidemia.Core.Models.Dto.External;

public record HttpEmailRequestDto
(
    string Name,
    string From,
    string To,
    string Subject,
    string Html
);