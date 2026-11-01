// Created on 29/10/2026 19:03 by Laserson

using Lidemia.Core.Models.Domain;

namespace Lidemia.Common.BusinessLogic.Services.Data.Abstract;

public interface IProductCategoryService
{
    Task<IEnumerable<ProductCategoryModel>> GetCategories(CancellationToken cancellationToken);
}