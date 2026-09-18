// Created on 03/09/2026 18:56 by Laserson

using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Repository.Abstract.Base;

namespace Lidemia.DataAccess.Repository.Abstract;

public interface IProductFeedbackRepository : IRepository<ProductFeedbackEntity, Guid>
{
    Task<IReadOnlyList<ProductFeedbackEntity>> GetRecentFeedback(long productId, int count = 5, CancellationToken cancellationToken = default);
}