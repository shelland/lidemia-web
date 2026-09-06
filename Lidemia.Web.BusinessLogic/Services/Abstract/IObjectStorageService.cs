// Created on 11/12/2025 23:08 by Laserson

using Lidemia.Core.Enums;
using Lidemia.Core.Models.Misc;

namespace Lidemia.Web.BusinessLogic.Services.Abstract;

public interface IObjectStorageService
{
    Task<ObjectStorageUploadResult> Upload(Stream content,
        ObjectStoragePayloadType type,
        string? fileName = null,
        IDictionary<string, string>? tags = null,
        CancellationToken cancellationToken = default);

    Task Delete(string bucket, string path, CancellationToken cancellationToken);

    Task<Stream> GetFileContent(string bucket, string path, CancellationToken cancellationToken);
}