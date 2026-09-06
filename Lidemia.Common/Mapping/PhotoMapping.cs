// Created on 04/09/2026 18:32 by Laserson

using Lidemia.Core.Models.Domain;
using Lidemia.DataAccess.Entities;

namespace Lidemia.Common.Mapping;

public static class PhotoMapping
{
    public static PhotoModel ToModel(this PhotoEntity entity, string storageHost)
    {
        return new PhotoModel();
    }
}