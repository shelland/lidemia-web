// Created on 02/09/2026 20:16 by Laserson

using Lidemia.Core.Enums;
using Lidemia.Core.Models.Base;
using Lidemia.DataAccess.Abstract;

namespace Lidemia.DataAccess.Entities;

public class ProductReviewEntity : IDbEntity, IHasId<long>
{
    public long Id { get; set; }

    public long ProductId { get; set; }

    public ProductEntity Product { get; set; } = null!;

    public long CustomerId { get; set; }

    public CustomerEntity Customer { get; set; } = null!;

    public string Review { get; set; } = string.Empty;

    public string? PositiveReview { get; set; }

    public string? NegativeReview { get; set; }

    public byte Rate { get; set; }

    public ProductReviewState State { get; set; }

    public bool IsAnonymous { get; set; }

    public string? SupplierFeedback { get; set; }

    public DateTimeOffset? SupplierFeedbackDate { get; set; }

    public int RowVersion { get; set; }

    public bool IsActive { get; set; }

    public DateTimeOffset CreateDate { get; set; }

    public DateTimeOffset? UpdateDate { get; set; }
}