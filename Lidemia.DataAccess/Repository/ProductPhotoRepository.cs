// Created on 20/10/2026 19:20 by Laserson

using Lidemia.DataAccess.Context;
using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Extensions;
using Lidemia.DataAccess.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.DataAccess.Repository;

[ServiceDescriptor<IProductPhotoRepository>(ServiceLifetime.Scoped)]
public class ProductPhotoRepository : IProductPhotoRepository
{
    private readonly LidemiaDbContext context;

    public ProductPhotoRepository(LidemiaDbContext context)
    {
        this.context = context;
    }

    public async Task AddProductPhoto(long userId, long productId, bool isPrimary, string filePath, CancellationToken cancellationToken)
    {
        var productPhoto = new ProductPhotoEntity
        {
            ProductId = productId,
            IsPrimary = isPrimary,
            Photo = new PhotoEntity
            {
                IsProcessed = true,
                ExtraPath = filePath,
                UserId = userId,
            }
        };

        this.context.ProductPhotos.Add(productPhoto);
        await this.context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<PhotoEntity>> GetProductPhotos(long productId, CancellationToken cancellationToken)
    {
        var query = this.context.ProductPhotos
            .AsActive()
            .Include(x => x.Photo)
            .Where(x => x.ProductId == productId)
            .Select(x => x.Photo);

        return await query.ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<int> DeleteProductPhotos(long productId, IEnumerable<long> photoIds, CancellationToken cancellationToken)
    {
        var ids = photoIds.Distinct().ToArray();

        if (ids.Length == 0)
        {
            return 0;
        }

        var deletedCount = await this.context.ProductPhotos
            .AsActive()
            .Where(x => x.ProductId == productId && ids.Contains(x.PhotoId))
            .ExecuteUpdateAsync(x => x.SetProperty(e => e.IsActive, false), cancellationToken: cancellationToken);

        if (deletedCount == 0)
        {
            return 0;
        }

        // The primary photo cannot point to a photo that is not attached to the product anymore
        await this.context.Products
            .AsActive()
            .Where(x => x.Id == productId && x.PrimaryPhotoId != null && ids.Contains(x.PrimaryPhotoId.Value))
            .ExecuteUpdateAsync(x => x.SetProperty(e => e.PrimaryPhotoId, (long?)null), cancellationToken: cancellationToken);

        return deletedCount;
    }
}