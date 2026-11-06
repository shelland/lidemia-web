// Created on 04/11/2026 14:22 by Laserson

using FluentResults;
using Lidemia.Core.Models.Service;

namespace Lidemia.Common.BusinessLogic.Services.Data.Abstract;

public interface ICreateOrderService
{
    Task<Result> Create(CreateOrderModel model, CancellationToken cancellationToken);
}