// Created on 02/09/2026 20:16 by Laserson

using Lidemia.Core.Models.Base;
using Lidemia.DataAccess.Abstract;

namespace Lidemia.DataAccess.Entities;

public class ProductFeedbackEntity : IDbEntity, IHasId<Guid>
{
    public Guid Id { get; set; }

    public long ProductId { get; set; }

    public ProductEntity Product { get; set; } = null!;

    public long CustomerId { get; set; }

    public CustomerEntity Customer { get; set; } = null!;

    public string? Comment { get; set; }

    public byte Rate { get; set; }

    public int RowVersion { get; set; }

    public bool IsActive { get; set; }

    public DateTimeOffset CreateDate { get; set; }

    public DateTimeOffset? UpdateDate { get; set; }
}