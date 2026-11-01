// Created on 30/10/2026 19:53 by Laserson

using Lidemia.Core.Models.Base;
using Lidemia.Core.Models.Misc;

namespace Lidemia.Core.Models.Domain;

public class ProductReviewModel : AbstractEntity<long>
{
    public LightCustomerModel Customer { get; set; } = null!;

    public string Review { get; set; } = string.Empty;

    public string? PositiveReview { get; set; }

    public string? NegativeRate { get; set; }

    public string? SupplierFeedback { get; set; }

    public DateTimeOffset? SupplierFeedbackDate { get; set; }

    public bool IsAnonymous { get; set; }

    public byte Rate { get; set; }
}