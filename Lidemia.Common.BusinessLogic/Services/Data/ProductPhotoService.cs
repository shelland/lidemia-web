// Created on 20/10/2026 19:55 by Laserson

using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Common.Mapper;
using Lidemia.Common.Mapper.Abstract;
using Lidemia.Core.Models.Domain;
using Lidemia.DataAccess.Repository.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.Common.BusinessLogic.Services.Data;

[ServiceDescriptor<IProductPhotoService>(ServiceLifetime.Scoped)]
public class ProductPhotoService : IProductPhotoService
{
    private readonly IProductPhotoRepository productPhotoRepository;
    private readonly IPhotoMapper photoMapper;

    public ProductPhotoService(IProductPhotoRepository productPhotoRepository, IPhotoMapper photoMapper)
    {
        this.productPhotoRepository = productPhotoRepository;
        this.photoMapper = photoMapper;
    }

    public Task AddProductPhoto(long userId, long productId, bool isPrimary, string filePath, CancellationToken cancellationToken)
    {
        return this.productPhotoRepository.AddProductPhoto(userId, productId, isPrimary, filePath, cancellationToken);
    }

    public async Task<IReadOnlyList<PhotoModel>> GetProductPhotos(long productId, CancellationToken cancellationToken)
    {
        var results = await this.productPhotoRepository.GetProductPhotos(productId, cancellationToken);
        return this.photoMapper.MapList(results);
    }

    public Task<int> RemoveProductPhotos(long productId, IEnumerable<long> photoIds, CancellationToken cancellationToken)
    {
        return this.productPhotoRepository.DeleteProductPhotos(productId, photoIds, cancellationToken);
    }
}