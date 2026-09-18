// Created on 01/09/2026 15:46 by Laserson

using FluentResults;
using Lidemia.Core.Enums;
using Lidemia.Core.Extensions;
using Lidemia.Core.Models.Dto;
using Lidemia.Core.Models.Misc;
using Lidemia.Core.Models.Service;
using Lidemia.DataAccess.Context;
using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Extensions;
using Lidemia.DataAccess.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using X.PagedList;

namespace Lidemia.DataAccess.Repository;

[ServiceDescriptor<IProductRepository>(ServiceLifetime.Scoped)]
public class ProductRepository : IProductRepository
{
    private readonly LidemiaDbContext context;

    public ProductRepository(LidemiaDbContext context)
    {
        this.context = context;
    }

    public Task<ProductEntity?> GetById(long key, CancellationToken cancellationToken)
    {
        return this.context.Products
            .AsActive()
            .FirstOrDefaultAsync(x => x.Id == key, cancellationToken: cancellationToken);
    }

    public Task<ProductEntity?> GetByIdPublic(long key, CancellationToken cancellation)
    {
        return this.context.Products.AsActive().FirstOrDefaultAsync(
            x => x.Id == key && x.IsVisible && x.Supplier.IsActive && x.Supplier.Status == SupplierStatus.Active,
            cancellationToken: cancellation);
    }

    public async Task<Result<long>> Save(long supplierId, SaveProductModel model, CancellationToken cancellationToken)
    {
        long productId;

        if (model.Id.HasValue)
        {
            var product = await this.context.Products.AsActive()
                .FirstOrDefaultAsync(x => x.Id == model.Id && x.SupplierId == supplierId, cancellationToken: cancellationToken);
            
            await this.context.Products.Where(x => x.Id == model.Id && x.SupplierId == supplierId)
                .ExecuteUpdateAsync(x => x.SetProperty(e => e.Title, model.Title), cancellationToken: cancellationToken);

            productId = model.Id.Value;
        }
        else
        {
            var product = new ProductEntity
            {
                SupplierId = supplierId,
                Title = model.Title
            };

            this.context.Products.Add(product);
            await this.context.SaveChangesAsync(cancellationToken);

            productId = product.Id;
        }

        return Result.Ok(productId);
    }

    public Task Delete(long key, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IPagedList<ProductEntity>> GetPublicList(ProductsListFilterModel filterModel, CancellationToken cancellationToken = default)
    {
        IQueryable<ProductEntity> query = this.context
            .Products
            .Include(x => x.Supplier)
            .Include(x => x.Category)
            .Include(x => x.PrimaryPhoto)
            .AsActive()
            .Where(x => x.IsVisible && x.Supplier.IsActive && x.Supplier.Status == SupplierStatus.Active)
            .OrderByDescending(x => x.Id);

        throw new NotImplementedException();
    }

    public async Task<IPagedList<ProductEntity>> GetSupplierProducts(long id, PagingInfoModel? pagingInfoModel = null,
        CancellationToken cancellationToken = default)
    {
        var products = await this.context.Products
            .AsActive().Where(x => x.SupplierId == id)
            .OrderByDescending(x => x.Id).ToPagedListEx(pagingInfoModel, cancellationToken);

        return products;
    }
}