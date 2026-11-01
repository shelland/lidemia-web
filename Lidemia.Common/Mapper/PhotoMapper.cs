// Created on 26/10/2026 19:35 by Laserson

using Lidemia.Common.Mapper.Abstract;
using Lidemia.Core.Models.Configuration;
using Lidemia.Core.Models.Domain;
using Lidemia.DataAccess.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Scrutor;

namespace Lidemia.Common.Mapper;

public interface IPhotoMapper : IMapper<PhotoEntity, PhotoModel>
{
    PhotoModel GetPlaceholder();
}

[ServiceDescriptor<IPhotoMapper>(ServiceLifetime.Scoped)]
public class PhotoMapper : IPhotoMapper
{
    private readonly IOptions<StorageSettingsModel> storageSettings;

    public PhotoMapper(IOptions<StorageSettingsModel> storageSettings)
    {
        this.storageSettings = storageSettings;
    }

    public PhotoModel Map(PhotoEntity source)
    {
        return new PhotoModel
        {
            Id = source.Id,
            OriginalFilePath = this.GetUrl(source.ExtraPath),
            LargeThumbPath = this.GetUrl(source.LargeThumbUrl),
            MediumThumbPath = this.GetUrl(source.MediumThumbUrl),
            SmallThumbPath = this.GetUrl(source.SmallThumbUrl),
            CreateDate = source.CreateDate
        };
    }

    public PhotoModel GetPlaceholder()
    {
        return new PhotoModel
        {
            OriginalFilePath = this.GetUrl(storageSettings.Value.ProductPlaceholderImageUrl)
        };
    }

    private string? GetUrl(string? relativePath)
    {
        if (string.IsNullOrEmpty(relativePath))
        {
            return null;
        }

        return $"{storageSettings.Value.StorageDomain}{relativePath}";
    }
}