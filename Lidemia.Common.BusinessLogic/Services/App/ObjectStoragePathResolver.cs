// Created on 12/12/2025 20:38 by Laserson

using Lidemia.Common.BusinessLogic.Services.App.Abstract;
using Lidemia.Core.Enums;
using Lidemia.Core.Models.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Scrutor;

namespace Lidemia.Common.BusinessLogic.Services.App;

[ServiceDescriptor<IObjectStoragePathResolver>(ServiceLifetime.Singleton)]
public class ObjectStoragePathResolver : IObjectStoragePathResolver
{
    private readonly IOptions<ApplicationSettingsModel> options;

    public ObjectStoragePathResolver(IOptions<ApplicationSettingsModel> options)
    {
        this.options = options;
    }

    public (string BucketName, string Path) Resolve(ObjectStoragePayloadType type, string fileName)
    {
        var bucket = this.options.Value.StorageBucket;
        return (bucket, fileName);
    }
}