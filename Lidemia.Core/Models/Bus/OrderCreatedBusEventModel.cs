// Created on 03/11/2026 17:53 by Laserson

namespace Lidemia.Core.Models.Bus;

public record OrderCreatedBusEventModel
(
    long Id,
    long CustomerId,
    IEnumerable<long> ProductIds
);