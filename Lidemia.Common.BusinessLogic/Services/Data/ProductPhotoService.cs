// Created on 20/10/2026 19:55 by Laserson

using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Common.Mapping;
using Lidemia.Core.Models.Configuration;
using Lidemia.Core.Models.Domain;
using Lidemia.DataAccess.Repository.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Scrutor;

namespace Lidemia.Common.BusinessLogic.Services.Data;

[ServiceDescriptor<IProductPhotoService>(ServiceLifetime.Scoped)]
public class ProductPhotoService : IProductPhotoService
{
    private readonly IProductPhotoRepository productPhotoRepository;
    private readonly IOptions<StorageSettingsModel> storageSettings;

    public ProductPhotoService(IProductPhotoRepository productPhotoRepository, IOptions<StorageSettingsModel> storageSettings)
    {
        this.productPhotoRepository = productPhotoRepository;
        this.storageSettings = storageSettings;
    }

    public Task AddProductPhoto(long userId, long productId, bool isPrimary, string filePath, CancellationToken cancellationToken)
    {
        return this.productPhotoRepository.AddProductPhoto(userId, productId, isPrimary, filePath, cancellationToken);
    }

    public async Task<IEnumerable<PhotoModel>> GetProductPhotos(long productId, CancellationToken cancellationToken)
    {
        var results = await this.productPhotoRepository.GetProductPhotos(productId, cancellationToken);
        return results.Select(x => x.ToModel(this.storageSettings.Value.StorageDomain));
    }
}