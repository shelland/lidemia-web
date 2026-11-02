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
            .Include(x => x.PrimaryPhoto)
            .Include(x => x.Category)
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
            await this.context.Products
                .Where(x => x.Id == model.Id && x.SupplierId == supplierId)
                .ExecuteUpdateAsync(x => x
                        .SetProperty(e => e.Title, model.Title)
                        .SetProperty(e => e.ParentId, model.ParentId)
                        .SetProperty(e => e.Description, model.Description)
                        .SetProperty(e => e.ShortDescription, model.ShortDescription)
                        .SetProperty(e => e.AvailabilityType, model.AvailabilityType)
                        .SetProperty(e => e.PriceUnit, model.PriceUnit)
                        .SetProperty(e => e.Price, e => model.Price ?? e.Price)
                        .SetProperty(e => e.MinQuantity, (double?)model.MinQuantity)
                        .SetProperty(e => e.MaxQuantity, (double?)model.MaxQuantity)
                        .SetProperty(e => e.Sku, model.Sku)
                        .SetProperty(e => e.DiscountPrice, model.DiscountPrice)
                        .SetProperty(e => e.HasCurrentDiscount, e => model.HasCurrentDiscount ?? e.HasCurrentDiscount)
                        .SetProperty(e => e.DiscountStartDate, model.DiscountStartDate)
                        .SetProperty(e => e.DiscountEndDate, model.DiscountEndDate)
                        .SetProperty(e => e.Tags, model.Tags ?? []),
                    cancellationToken: cancellationToken);

            productId = model.Id.Value;
        }
        else
        {
            var product = new ProductEntity
            {
                SupplierId = supplierId,
                Title = model.Title,
                ParentId = model.ParentId,
                Description = model.Description,
                ShortDescription = model.ShortDescription,
                AvailabilityType = model.AvailabilityType,
                PriceUnit = model.PriceUnit,
                Price = model.Price ?? 0,
                MinQuantity = (double?)model.MinQuantity,
                MaxQuantity = (double?)model.MaxQuantity,
                Sku = model.Sku,
                DiscountPrice = model.DiscountPrice,
                HasCurrentDiscount = model.HasCurrentDiscount ?? false,
                DiscountStartDate = model.DiscountStartDate,
                DiscountEndDate = model.DiscountEndDate,
                Tags = model.Tags ?? []
            };

            this.context.Products.Add(product);
            await this.context.SaveChangesAsync(cancellationToken);

            productId = product.Id;
        }

        await this.SetPrimaryPhoto(productId, model.PrimaryPhotoId, cancellationToken);

        return Result.Ok(productId);
    }

    private async Task SetPrimaryPhoto(long productId, long? photoId, CancellationToken cancellationToken)
    {
        if (!photoId.HasValue)
        {
            return;
        }

        var value = photoId.Value;

        // The primary photo must be one of the photos that belong to the product
        var belongsToProduct = await this.context.ProductsPhotos
            .AsActive()
            .AnyAsync(x => x.ProductId == productId && x.PhotoId == value, cancellationToken);

        if (!belongsToProduct)
        {
            return;
        }

        await this.context.Products
            .Where(x => x.Id == productId)
            .ExecuteUpdateAsync(x => x.SetProperty(e => e.PrimaryPhotoId, photoId), cancellationToken: cancellationToken);
    }

    public async Task<Result<(int Enabled, int Disabled)>> EnablePendingDiscounts(DateTimeOffset now, CancellationToken cancellationToken)
    {
        var enabledCount = await this.context.Products
            .AsActive()
            .Where(x => x.IsVisible && !x.HasCurrentDiscount && x.DiscountStartDate >= now)
            .ExecuteUpdateAsync(x => x.SetProperty(e => e.HasCurrentDiscount, true), cancellationToken: cancellationToken);

        var disabledCount = await this.context.Products
            .AsActive()
            .Where(x => x.IsVisible && x.HasCurrentDiscount && x.DiscountEndDate <= now)
            .ExecuteUpdateAsync(x =>
                x.SetProperty(e => e.HasCurrentDiscount, false)
                    .SetProperty(e => e.DiscountStartDate, (DateTimeOffset?)null)
                    .SetProperty(e => e.DiscountEndDate, (DateTimeOffset?)null), cancellationToken: cancellationToken);

        return Result.Ok((enabledCount, disabledCount));
    }

    public Task SetProductVisibility(long productId, bool isVisible, CancellationToken cancellationToken)
    {
        return this.context.Products
            .AsActive()
            .Where(x => x.Id == productId)
            .ExecuteUpdateAsync(x => x.SetProperty(e => e.IsVisible, isVisible), cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<ProductEntity>> GetProducts(IEnumerable<long> ids, CancellationToken cancellationToken)
    {
        var products = await this.context
            .Products
            .AsActive()
            .Include(x => x.PrimaryPhoto)
            .Where(x => ids.Contains(x.Id))
            .ToListAsync(cancellationToken: cancellationToken);

        return products;
    }

    public async Task<IReadOnlyList<LightProductModel>> GetRelatedProducts(long categoryId, int count = 3, CancellationToken cancellationToken = default)
    {
        var products = await this.context.Products
            .AsActive()
            .Where(x => x.CategoryId == categoryId)
            .Select(x => new LightProductModel(x.Id, x.Title!, x.Price))
            .OrderBy(x => Guid.NewGuid())
            .Take(count)
            .ToListAsync(cancellationToken: cancellationToken);

        return products;
    }

    public async Task<IEnumerable<ProductEntity>> GetProductsForSearchIndex(CancellationToken cancellationToken)
    {
        return await this.context.Products.AsActive().Where(x => x.Supplier.Status == SupplierStatus.Active).ToListAsync(cancellationToken);
    }

    public Task Delete(long key, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IPagedList<ProductEntity>> GetPublicList(ProductsListFilterModel filterModel, long[] searchIds,
        CancellationToken cancellationToken)
    {
        IQueryable<ProductEntity> query = this.context
            .Products
            .Include(x => x.Supplier)
            .Include(x => x.Category)
            .Include(x => x.PrimaryPhoto)
            .AsActive()
            .Where(x => x.IsVisible && x.Supplier.IsActive && x.Supplier.Status == SupplierStatus.Active)
            .OrderByDescending(x => x.Id);

        if (searchIds.Length > 0)
        {
            query = query.Where(x => searchIds.Contains(x.Id));
        }

        return query.ToPagedListEx(filterModel, cancellationToken);
    }

    public async Task<IPagedList<ProductEntity>> GetSupplierProducts(long id, PagingInfoModel? pagingInfoModel = null,
        CancellationToken cancellationToken = default)
    {
        var products = await this.context.Products
            .AsActive().Where(x => x.SupplierId == id)
            .OrderByDescending(x => x.Id)
            .ToPagedListEx(pagingInfoModel, cancellationToken);

        return products;
    }
}