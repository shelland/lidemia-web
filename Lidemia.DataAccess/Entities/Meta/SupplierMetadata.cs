// Created on 07/12/2021 21:44 by Andrey Laserson

using Lidemia.Core.Enums;

namespace Lidemia.DataAccess.Entities.Meta;

public class SupplierMetadata
{
    public static SupplierMetadata Default => new();

    public SupplierFlags[] Flags { get; set; } = [];
}