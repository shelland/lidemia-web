// Created on 02/09/2026 20:15 by Laserson

using Lidemia.Core.Models.Domain;

namespace Lidemia.Common.BusinessLogic.Services.Data.Abstract;

public interface IPhotoService
{
    Task<IEnumerable<PhotoModel>> GetProductPhotos(long productId, CancellationToken cancellationToken);
}