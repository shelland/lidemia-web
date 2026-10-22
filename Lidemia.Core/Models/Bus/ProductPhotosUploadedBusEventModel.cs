// Created on 20/10/2026 18:37 by Laserson

using Lidemia.Core.Models.Misc;

namespace Lidemia.Core.Models.Bus;

public record ProductPhotosUploadedBusEventModel
(
    long ProductId,
    long UserId,
    bool IsPrimary,
    IReadOnlyList<ObjectStorageUploadResult> Results
);