// Created on 20/10/2026 18:37 by Laserson

using Lidemia.Common.BusinessLogic.Services.App.Abstract;
using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Core.Enums;
using Lidemia.Core.Extensions;
using Lidemia.Core.Models.Bus;
using Lidemia.Storage.Logic.Services.Abstract;
using MassTransit;

namespace Lidemia.Storage.Logic.Consumers;

public class ProductPhotosUploadedEventConsumer : IConsumer<ProductPhotosUploadedBusEventModel>
{
    private readonly IProductPhotoService productPhotoService;
    private readonly IPhotoPathProvider photoPathProvider;
    private readonly IObjectStorageService objectStorageService;
    private readonly ILogger<ProductPhotosUploadedEventConsumer> logger;

    public ProductPhotosUploadedEventConsumer(
        IProductPhotoService productPhotoService,
        IPhotoPathProvider photoPathProvider,
        IObjectStorageService objectStorageService,
        ILogger<ProductPhotosUploadedEventConsumer> logger)
    {
        this.productPhotoService = productPhotoService;
        this.photoPathProvider = photoPathProvider;
        this.objectStorageService = objectStorageService;
        this.logger = logger;
    }

    public async Task Consume(ConsumeContext<ProductPhotosUploadedBusEventModel> context)
    {
        foreach (var photo in context.Message.Results)
        {
            this.logger.LogInformation("Start processing photo {Path}", photo.ObjectName);

            await using var content =
                await this.objectStorageService.GetFileContent(ObjectStoragePayloadType.ProductPhoto, photo.ObjectName, context.CancellationToken);
            var path = this.photoPathProvider.GetStoragePath();
            
            var fileInfo = new FileInfo(path.FullPath);
            fileInfo.Directory.NotNull().Create();

            await using var fileStream = new FileStream(path.FullPath, FileMode.Create);
            content.Position = 0;
            await content.CopyToAsync(fileStream);
            fileStream.Close();

            await this.productPhotoService.AddProductPhoto(context.Message.UserId, context.Message.ProductId, context.Message.IsPrimary, path.RelativePath,
                context.CancellationToken);

            this.logger.LogInformation("Written a new photo file to {Path}", path.FullPath);
        }
    }
}