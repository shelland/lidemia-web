// Created on 01/09/2026 14:39 by Laserson

using Lidemia.Core.Models.Dto;
using Lidemia.Core.Models.Misc;
using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Repository.Abstract.Base;
using X.PagedList;

namespace Lidemia.DataAccess.Repository.Abstract;

public interface IProductRepository : IRepository<ProductEntity, long>
{
    Task<IPagedList<ProductEntity>> GetPublicList(ProductsListFilterModel filterModel, CancellationToken cancellationToken = default);

    Task<IPagedList<ProductEntity>> GetSupplierProducts(long id, PagingInfoModel? pagingInfoModel = null, CancellationToken cancellationToken = default);

    Task<ProductEntity?> GetByIdPublic(long key, CancellationToken cancellation);
}