// Created on 03/09/2026 18:53 by Laserson

using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Repository.Abstract.Base;

namespace Lidemia.DataAccess.Repository.Abstract;

public interface IPhotoRepository : IRepository<PhotoEntity, long>
{
    Task<IEnumerable<PhotoEntity>> GetProductPhotos(long productId, CancellationToken cancellationToken);
}