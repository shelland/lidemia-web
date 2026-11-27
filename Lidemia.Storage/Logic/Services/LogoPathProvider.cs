// Created on 25/11/2026 18:10 by Laserson

using Lidemia.Storage.Logic.Services.Abstract;
using Scrutor;

namespace Lidemia.Storage.Logic.Services;

[ServiceDescriptor<ILogoPathProvider>(ServiceLifetime.Singleton)]
public class LogoPathProvider : ILogoPathProvider
{
    private const string StorageDirectory = "logos";

    private readonly IWebHostEnvironment webHostEnvironment;

    public LogoPathProvider(IWebHostEnvironment webHostEnvironment)
    {
        this.webHostEnvironment = webHostEnvironment;
    }

    public (string FullPath, string RelativePath) GetStoragePath(long supplierId, string objectName)
    {
        var rootDirectory = this.webHostEnvironment.WebRootPath;
        var id = Guid.NewGuid().ToString("N");
        var extension = Path.GetExtension(objectName);
        var fileName = $"{id}{extension}";

        var relativePath = $"/{StorageDirectory}/{supplierId}/{fileName}";
        var fullPath = Path.Combine(rootDirectory, StorageDirectory, supplierId.ToString(), fileName);

        return (fullPath, relativePath);
    }
}