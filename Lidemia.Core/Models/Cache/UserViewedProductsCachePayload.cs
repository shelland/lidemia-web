// Created on 25/11/2026 23:01 by Laserson

namespace Lidemia.Core.Models.Cache;

public record UserViewedProductsCachePayload
(
    IDictionary<long, DateTimeOffset> Items
);