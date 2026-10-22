// Created on 11/12/2025 23:08 by Laserson

using Lidemia.Core.Enums;
using Lidemia.Core.Models.Misc;

namespace Lidemia.Common.BusinessLogic.Services.App.Abstract;

public interface IObjectStorageService
{
    Task<ObjectStorageUploadResult> Upload(Stream content,
        ObjectStoragePayloadType type,
        string? fileName = null,
        IDictionary<string, string>? tags = null,
        CancellationToken cancellationToken = default);

    Task Delete(string bucket, string path, CancellationToken cancellationToken);

    Task<Stream> GetFileContent(ObjectStoragePayloadType type, string fileName, CancellationToken cancellationToken);
}