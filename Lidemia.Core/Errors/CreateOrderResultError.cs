// Created on 09/11/2026 15:43 by Laserson

using FluentResults;

namespace Lidemia.Core.Errors;

public class CreateOrderResultError : Error
{
    public CreateOrderResultError(IReadOnlyCollection<long> failedProducts)
    {
        FailedProducts = failedProducts;
    }

    public IReadOnlyCollection<long> FailedProducts { get; }
}