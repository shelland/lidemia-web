// Created on 05/10/2026 by Laserson

using Lidemia.Core.Models.Dto;
using Lidemia.Core.Models.Misc;

namespace Lidemia.Search.Services.Abstract;

public interface ISearchService
{
    Task<IEnumerable<ProductMainInfoModel>> Search(ProductsListFilterModel filter, CancellationToken cancellationToken = default);

    Task<bool> UpdateInIndex(ProductMainInfoModel model, CancellationToken cancellationToken = default);

    Task<bool> RemoveFromIndex(long id, CancellationToken cancellationToken = default);

    Task<bool> RebuildIndex(IEnumerable<ProductMainInfoModel> products, CancellationToken cancellationToken = default);
}