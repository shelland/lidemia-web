// Created on 25/11/2026 17:57 by Laserson

using Lidemia.Core.Models.Misc;

namespace Lidemia.Core.Models.Bus;

public record SupplierLogoUploadedBusEventModel
(
    long SupplierId,
    ObjectStorageUploadResult Result
);