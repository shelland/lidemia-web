// Created on 01/09/2026 15:46 by Laserson

using FluentResults;
using Lidemia.Core.Enums;
using Lidemia.Core.Errors;
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
            .Include(x => x.Supplier)
            .ThenInclude(x => x.User)
            .Include(x => x.PrimaryPhoto)
            .Include(x => x.Category)
            .Include(x => x.ReviewStats)
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
            var affectedRows = await this.context.Products
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
                        .SetProperty(e => e.Tags, model.Tags ?? [])
                        .SetProperty(e => e.DeliveryTypes, model.DeliveryTypes)
                        .SetProperty(e => e.Slug, model.Slug),
                    cancellationToken: cancellationToken);

            if (affectedRows == 0)
            {
                return Result.Fail(CommonErrors.NotFoundError);
            }

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
                Tags = model.Tags ?? [],
                DeliveryTypes = model.DeliveryTypes,
                Slug = model.Slug
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
        var belongsToProduct = await this.context.ProductPhotos
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
            .Include(x => x.Supplier)
            .ThenInclude(x => x.User)
            .Where(x => ids.Contains(x.Id))
            .ToListAsync(cancellationToken: cancellationToken);

        return products;
    }

    public async Task<IReadOnlyList<LightProductModel>> GetRelatedProducts(long categoryId, long currentProductId, int count = 3,
        CancellationToken cancellationToken = default)
    {
        var products = await this.context.Products
            .AsActive()
            .Where(x => x.CategoryId == categoryId && x.Id != currentProductId)
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

    public async Task<IReadOnlyCollection<long>> ReduceQty(IDictionary<long, int> items, CancellationToken cancellationToken)
    {
        var ids = items.Select(x => x.Key);

        var products = this.context.Products.AsTracking().Where(x => ids.Contains(x.Id));
        var failedProducts = new List<long>();

        foreach (var product in products)
        {
            if (product.AvailableUnits == null)
            {
                continue;
            }

            var newQty = product.AvailableUnits - (items[product.Id] * (product.QuantityReducing ?? 1));

            if (newQty < 0)
            {
                failedProducts.Add(product.Id);
                continue;
            }

            product.AvailableUnits = newQty;
        }

        await this.context.SaveChangesAsync(cancellationToken);
        return failedProducts;
    }

    public async Task<long> GetProductSupplier(long productId, CancellationToken cancellationToken)
    {
        return await this.context.Products
            .Where(x => x.Id == productId)
            .Select(x => x.SupplierId)
            .FirstAsync(cancellationToken: cancellationToken);
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
            .ThenInclude(x => x.User)
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

    public async Task<IReadOnlyList<ProductEntity>> GetSupplierCatalog(long supplierId, int limit, CancellationToken cancellationToken)
    {
        return await this.context.Products
            .AsActive()
            .Include(x => x.Supplier)
            .ThenInclude(x => x.User)
            .Include(x => x.PrimaryPhoto)
            .Include(x => x.Category)
            .Where(x => x.SupplierId == supplierId && x.IsVisible)
            .OrderBy(x => x.Title)
            .Take(limit)
            .ToListAsync(cancellationToken);
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