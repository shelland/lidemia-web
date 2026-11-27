// Created on 25/11/2026 17:57 by Laserson

using Lidemia.Common.BusinessLogic.Services.App.Abstract;
using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Core.Enums;
using Lidemia.Core.Models.Bus;
using Lidemia.Storage.Logic.Services.Abstract;
using MassTransit;

namespace Lidemia.Storage.Logic.Consumers;

public class SupplierLogoUploadedEventConsumer : IConsumer<SupplierLogoUploadedBusEventModel>
{
    private readonly IObjectStorageService objectStorageService;
    private readonly ISupplierService supplierService;
    private readonly ILogoPathProvider logoPathProvider;
    private readonly ILogger<SupplierLogoUploadedEventConsumer> logger;

    public SupplierLogoUploadedEventConsumer(
        IObjectStorageService objectStorageService,
        ISupplierService supplierService,
        ILogoPathProvider logoPathProvider,
        ILogger<SupplierLogoUploadedEventConsumer> logger)
    {
        this.objectStorageService = objectStorageService;
        this.supplierService = supplierService;
        this.logoPathProvider = logoPathProvider;
        this.logger = logger;
    }

    public async Task Consume(ConsumeContext<SupplierLogoUploadedBusEventModel> context)
    {
        var message = context.Message;

        this.logger.LogInformation("Start processing a logo for supplier {SupplierId} from {Path}", message.SupplierId, message.Result.ObjectName);

        await using var content =
            await this.objectStorageService.GetFileContent(ObjectStoragePayloadType.SupplierLogo, message.Result.ObjectName, context.CancellationToken);

        var path = this.logoPathProvider.GetStoragePath(message.SupplierId, message.Result.ObjectName);
        var fileInfo = new FileInfo(path.FullPath);

        if (fileInfo.Directory is { Exists: false })
        {
            fileInfo.Directory.Create();
        }

        await using (var fileStream = new FileStream(path.FullPath, FileMode.Create))
        {
            content.Position = 0;
            await content.CopyToAsync(fileStream, context.CancellationToken);
        }

        var updated = await this.supplierService.UpdateLogoUrl(message.SupplierId, path.RelativePath, context.CancellationToken);

        if (!updated)
        {
            this.logger.LogWarning("The logo for supplier {SupplierId} was stored to {Path}, but the supplier was not found", message.SupplierId, path.FullPath);
            return;
        }

        this.logger.LogInformation("Written a new logo file for supplier {SupplierId} to {Path}", message.SupplierId, path.FullPath);
    }
}