// Created on 21/10/2026 21:36 by Laserson

namespace Lidemia.Storage.Logic.Services.Abstract;

public interface IPhotoPathProvider
{
    (string FullPath, string RelativePath) GetStoragePath();
}