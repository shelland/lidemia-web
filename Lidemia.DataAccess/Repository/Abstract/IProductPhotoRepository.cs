// Created on 20/10/2026 19:20 by Laserson

using Lidemia.Core.Models.Misc;
using Lidemia.DataAccess.Entities;

namespace Lidemia.DataAccess.Repository.Abstract;

public interface IProductPhotoRepository
{
    Task AddProductPhoto(long userId, long productId, bool isPrimary, string filePath, CancellationToken cancellationToken);

    Task<IEnumerable<PhotoEntity>> GetProductPhotos(long productId, CancellationToken cancellationToken);
}