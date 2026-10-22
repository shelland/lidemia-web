// Created on 20/10/2026 19:38 by Laserson

namespace Lidemia.Core.Models.Misc;

public record ImagePathsModel
(
    string? OriginalFilePath,
    string? LargeThumbPath,
    string? MediumThumbPath,
    string? SmallThumbPath
);