// Created on 02/09/2026 20:09 by Laserson

using FluentResults;
using Lidemia.Core.Models.Domain;
using Lidemia.Core.Models.Dto;
using Lidemia.Core.Models.Misc;

namespace Lidemia.Common.BusinessLogic.Services.Data.Abstract;

public interface IProductService
{
    Task<ProductModel?> GetById(long id, CancellationToken cancellationToken);

    Task<BasePagedListModel<ProductModel>> GetPublicList(ProductsListFilterModel filter, CancellationToken cancellationToken);

    Task<BasePagedListModel<ProductModel>> GetSupplierProducts(long id, PagingInfoModel pagingInfo, CancellationToken cancellationToken);

    Task<Result<long>> Save(long supplierId, SaveProductRequestDto request, CancellationToken cancellationToken);

    Task<Result<(int Enabled, int Disabled)>> EnablePendingDiscounts(CancellationToken cancellationToken);

    Task SetProductVisibility(long supplierId, SetProductVisibilityRequestDto request, CancellationToken cancellationToken);
}