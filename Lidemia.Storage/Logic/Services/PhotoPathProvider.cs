// Created on 21/10/2026 21:36 by Laserson

using Lidemia.Storage.Logic.Services.Abstract;
using Scrutor;

namespace Lidemia.Storage.Logic.Services;

[ServiceDescriptor<IPhotoPathProvider>(ServiceLifetime.Singleton)]
public class PhotoPathProvider : IPhotoPathProvider
{
    private readonly IWebHostEnvironment webHostEnvironment;

    public PhotoPathProvider(IWebHostEnvironment webHostEnvironment)
    {
        this.webHostEnvironment = webHostEnvironment;
    }

    public (string FullPath, string RelativePath) GetStoragePath()
    {
        var rootDirectory = this.webHostEnvironment.WebRootPath;
        var id = Guid.NewGuid().ToString("N");
        var fileName = $"{id}.jpg";

        var relativePath = Path.Combine("product-photos", id[0].ToString(), id[1].ToString(), id[2].ToString(), fileName);
        var urlPath = $"/product-photos/{id[0]}/{id[1]}/{id[2]}/{fileName}";

        var fullPath = Path.Combine(rootDirectory, relativePath);

        return (fullPath, urlPath);
    }
}