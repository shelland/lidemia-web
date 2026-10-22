// Created on 20/10/2026 19:55 by Laserson

using Lidemia.Core.Models.Domain;
using Lidemia.Core.Models.Misc;

namespace Lidemia.Common.BusinessLogic.Services.Data.Abstract;

public interface IProductPhotoService
{
    Task AddProductPhoto(long userId, long productId, bool isPrimary, string path, CancellationToken cancellationToken);

    Task<IEnumerable<PhotoModel>> GetProductPhotos(long productId, CancellationToken cancellationToken);
}