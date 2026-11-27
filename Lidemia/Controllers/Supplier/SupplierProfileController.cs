// Created on 22/11/2026 14:35 by Laserson

using Lidemia.Common.BusinessLogic.Services.App.Abstract;
using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Core.Attributes;
using Lidemia.Core.Enums;
using Lidemia.Core.Models.Bus;
using Lidemia.Core.Models.Misc;
using Lidemia.Core.Models.Service;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Lidemia.Controllers.Supplier;

[Route("Supplier/Profile")]
[SupplierAuthorize]
public class SupplierProfileController : BaseController
{
    private const long MaxLogoSize = 5 * 1024 * 1024;
    private static readonly string[] AllowedLogoExtensions = [".jpg", ".jpeg", ".png", ".gif", ".webp", ".bmp"];
    private readonly ISupplierService supplierService;
    private readonly IObjectStorageService objectStorageService;
    private readonly IPublishEndpoint publishEndpoint;
    private readonly ILogger<SupplierProfileController> logger;

    public SupplierProfileController(
        ISupplierService supplierService,
        IObjectStorageService objectStorageService,
        IPublishEndpoint publishEndpoint,
        ILogger<SupplierProfileController> logger)
    {
        this.supplierService = supplierService;
        this.objectStorageService = objectStorageService;
        this.publishEndpoint = publishEndpoint;
        this.logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View();
    }

    [HttpGet("Edit")]
    public async Task<IActionResult> Edit(CancellationToken cancellationToken)
    {
        var supplier = await this.supplierService.FindById(RequireEntityId, cancellationToken);

        if (supplier == null)
        {
            return NotFoundView;
        }

        return View(supplier);
    }

    [HttpPost("Logo")]
    public async Task<ResultInfo> UploadLogo([FromForm] IFormFile? file, CancellationToken cancellationToken)
    {
        if (file is not { Length: > 0 })
        {
            return new ErrorResultInfo(["errors.supplier.logo.fileEmpty"]);
        }

        if (file.Length > MaxLogoSize)
        {
            return new ErrorResultInfo(["errors.supplier.logo.fileTooLarge"]);
        }

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!AllowedLogoExtensions.Contains(extension))
        {
            return new ErrorResultInfo(["errors.supplier.logo.unsupportedFormat"]);
        }

        var supplierId = RequireEntityId;
        var fileName = $"{supplierId}-{Guid.NewGuid():N}{extension}";

        var tags = new Dictionary<string, string>
        {
            ["supplier-id"] = supplierId.ToString()
        };

        await using var content = file.OpenReadStream();

        var uploadResult = await this.objectStorageService.Upload(content, ObjectStoragePayloadType.SupplierLogo, fileName, tags, cancellationToken);

        this.logger.LogInformation("Uploaded a new logo for supplier {SupplierId} to {ObjectName}", supplierId, uploadResult.ObjectName);

        await this.publishEndpoint.Publish(new SupplierLogoUploadedBusEventModel(supplierId, uploadResult), cancellationToken);

        return new ResultInfo<ObjectStorageUploadResult>(uploadResult);
    }
}