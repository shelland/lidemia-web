// Created on 04/10/2026 14:50 by Laserson

namespace Lidemia.Core.Models.Misc;

public record ProductMainInfoModel
(
    long Id,

    string Title,

    decimal Price,

    long SupplierId,

    string Description,

    long CategoryId,

    string[] Tags,

    bool IsVisible
);