// Created on 25/11/2026 18:10 by Laserson

namespace Lidemia.Storage.Logic.Services.Abstract;

public interface ILogoPathProvider
{
    (string FullPath, string RelativePath) GetStoragePath(long supplierId, string objectName);
}