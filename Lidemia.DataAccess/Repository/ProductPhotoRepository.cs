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

        this.context.ProductsPhotos.Add(productPhoto);
        await this.context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<PhotoEntity>> GetProductPhotos(long productId, CancellationToken cancellationToken)
    {
        var query = this.context.ProductsPhotos
            .AsActive()
            .Include(x => x.Photo)
            .Where(x => x.ProductId == productId)
            .Select(x => x.Photo);

        return await query.ToListAsync(cancellationToken: cancellationToken);
    }
}