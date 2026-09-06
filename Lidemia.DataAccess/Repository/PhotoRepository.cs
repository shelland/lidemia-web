// Created on 03/09/2026 18:54 by Laserson

using Lidemia.DataAccess.Context;
using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Extensions;
using Lidemia.DataAccess.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.DataAccess.Repository;

[ServiceDescriptor<IPhotoRepository>(ServiceLifetime.Scoped)]
public class PhotoRepository : IPhotoRepository
{
    private readonly LidemiaDbContext context;

    public PhotoRepository(LidemiaDbContext context)
    {
        this.context = context;
    }

    public Task<PhotoEntity?> GetById(long key, CancellationToken cancellationToken)
    {
        return this.context.Photos.FirstOrDefaultAsync(x => x.Id == key, cancellationToken);
    }

    public Task Delete(long key, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
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