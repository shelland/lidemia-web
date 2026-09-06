// Created on 30/11/2021 21:28 by Andrey Laserson

using Lidemia.Core.Models.Base;
using Lidemia.DataAccess.Abstract;

namespace Lidemia.DataAccess.Entities;

public class ProductCategoryEntity : IDbEntity, IHasId<long>
{
    public long Id { get; set; }

    public string NameKey { get; set; } = string.Empty;

    public string? DescriptionKey { get; set; }

    public string? ImageClass { get; set; }

    public long? ParentId { get; set; }

    public ProductCategoryEntity? Parent { get; set; }

    public int RowVersion { get; set; }

    public bool IsActive { get; set; }

    public DateTimeOffset CreateDate { get; set; }

    public DateTimeOffset? UpdateDate { get; set; }
}