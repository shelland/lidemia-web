// Created on 26/10/2026 19:55 by Laserson

using Lidemia.Common.Mapper.Abstract;
using Lidemia.Core.Models.Domain;
using Lidemia.DataAccess.Entities;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.Common.Mapper;

public interface ISupplierMapper : IMapper<SupplierEntity, SupplierModel>;

[ServiceDescriptor<ISupplierMapper>(ServiceLifetime.Scoped)]
public class SupplierMapper : ISupplierMapper
{
    private readonly IUserMapper userMapper;

    public SupplierMapper(IUserMapper userMapper)
    {
        this.userMapper = userMapper;
    }

    public SupplierModel Map(SupplierEntity source)
    {
        return new SupplierModel
        {
            Id = source.Id,
            Name = source.Name,
            CreateDate = source.CreateDate,
            User = this.userMapper.Map(source.User)
        };
    }
}