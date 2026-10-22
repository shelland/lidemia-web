// Created on 03/09/2026 18:53 by Laserson

using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Core.Models.Configuration;
using Lidemia.DataAccess.Repository.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Scrutor;

namespace Lidemia.Common.BusinessLogic.Services.Data;

[ServiceDescriptor<IPhotoService>(ServiceLifetime.Scoped)]
public class PhotoService : IPhotoService
{
    private readonly IPhotoRepository photoRepository;
    private readonly IOptions<StorageSettingsModel> storageSettings;

    public PhotoService(IPhotoRepository photoRepository, IOptions<StorageSettingsModel> storageSettings)
    {
        this.photoRepository = photoRepository;
        this.storageSettings = storageSettings;
    }
}