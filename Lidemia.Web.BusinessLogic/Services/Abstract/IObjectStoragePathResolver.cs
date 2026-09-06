// Created on 12/12/2025 20:37 by Laserson

using Lidemia.Core.Enums;

namespace Lidemia.Web.BusinessLogic.Services.Abstract;

public interface IObjectStoragePathResolver
{
    (string BucketName, string Path) Resolve(ObjectStoragePayloadType type, string fileName);
}