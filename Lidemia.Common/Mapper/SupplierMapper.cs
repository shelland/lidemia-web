// Created on 26/10/2026 19:55 by Laserson

using Lidemia.Common.Mapper.Abstract;
using Lidemia.Core.Models.Configuration;
using Lidemia.Core.Models.Domain;
using Lidemia.DataAccess.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Scrutor;

namespace Lidemia.Common.Mapper;

public interface ISupplierMapper : IMapper<SupplierEntity, SupplierModel>;

[ServiceDescriptor<ISupplierMapper>(ServiceLifetime.Scoped)]
public class SupplierMapper : ISupplierMapper
{
    private readonly IUserMapper userMapper;
    private readonly ISupplierStatsMapper supplierStatsMapper;

    private readonly IOptions<StorageSettingsModel> storageSettings;

    public SupplierMapper(IUserMapper userMapper, ISupplierStatsMapper supplierStatsMapper, IOptions<StorageSettingsModel> storageSettings)
    {
        this.userMapper = userMapper;
        this.supplierStatsMapper = supplierStatsMapper;
        this.storageSettings = storageSettings;
    }

    public SupplierModel? Map(SupplierEntity? source)
    {
        if (source == null)
        {
            return null;
        }

        return new SupplierModel
        {
            Id = source.Id,
            Name = source.Name,
            CreateDate = source.CreateDate,
            User = this.userMapper.Map(source.User),
            Stats = this.supplierStatsMapper.Map(source.Stats),
            LogoUrl = source.LogoUrl != null ? GetUrl(source.LogoUrl) : null,
            Metadata = source.Metadata
        };
    }

    private string? GetUrl(string? relativePath)
    {
        if (string.IsNullOrEmpty(relativePath))
        {
            return null;
        }

        return $"{storageSettings.Value.StorageDomain}{relativePath}";
    }
}