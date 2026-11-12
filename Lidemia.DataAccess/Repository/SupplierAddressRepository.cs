// Created on 18/09/2026 19:24 by Laserson

using FluentResults;
using Lidemia.Core.Errors;
using Lidemia.Core.Extensions;
using Lidemia.Core.Models.Misc;
using Lidemia.Core.Models.Service;
using Lidemia.DataAccess.Context;
using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Extensions;
using Lidemia.DataAccess.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using X.PagedList;

namespace Lidemia.DataAccess.Repository;

[ServiceDescriptor<ISupplierAddressRepository>(ServiceLifetime.Scoped)]
public class SupplierAddressRepository : ISupplierAddressRepository
{
    private readonly LidemiaDbContext context;

    public SupplierAddressRepository(LidemiaDbContext context)
    {
        this.context = context;
    }

    public Task<SupplierAddressEntity?> GetById(long key, CancellationToken cancellationToken)
    {
        return this.context.SupplierAddresses
            .Include(x => x.Address)
            .ThenInclude(x => x.Country)
            .Include(x => x.Address)
            .ThenInclude(x => x.Region)
            .Include(x => x.Address)
            .ThenInclude(x => x.City)
            .AsActive()
            .FirstOrDefaultAsync(x => x.Id == key, cancellationToken: cancellationToken);
    }

    public Task Delete(long key, CancellationToken cancellationToken)
    {
        return this.context.SupplierAddresses
            .AsActive()
            .Where(x => x.Id == key)
            .ExecuteUpdateAsync(x => x.SetProperty(e => e.IsActive, false), cancellationToken);
    }

    public async Task<Result<long>> Save(SaveSupplierAddressModel model, CancellationToken cancellationToken)
    {
        if (model.Id != null)
        {
            var supplierAddress = await this.context.SupplierAddresses
                .AsTracking()
                .Include(x => x.Address)
                .FirstOrDefaultAsync(x => x.Id == model.Id && x.SupplierId == model.SupplierId, cancellationToken: cancellationToken);

            if (supplierAddress == null)
            {
                return Result.Fail(CommonErrors.NotFoundError);
            }

            supplierAddress.Comment = model.Comment;
            supplierAddress.Address.CountryId = model.CountryId;
            supplierAddress.Address.RegionId = model.RegionId;
            supplierAddress.Address.CityId = model.CityId;
            supplierAddress.Address.StreetName = model.StreetName;
            supplierAddress.Address.HouseAdd1 = model.HouseNumber;
            supplierAddress.Address.FlatNumber = model.FlatNumber;
            supplierAddress.Type = model.Type;

            await this.context.SaveChangesAsync(cancellationToken);
            return Result.Ok(supplierAddress.Id);
        }

        var entity = new SupplierAddressEntity
        {
            SupplierId = model.SupplierId,
            Comment = model.Comment,
            Type = model.Type,
            Address = new AddressEntity
            {
                CityId = model.CityId,
                RegionId = model.RegionId,
                CountryId = model.CountryId,
                FlatNumber = model.FlatNumber,
                HouseNumber = model.HouseNumber,
                StreetName = model.StreetName
            }
        };

        this.context.SupplierAddresses.Add(entity);
        await this.context.SaveChangesAsync(cancellationToken);

        return Result.Ok(entity.Id);
    }

    public async Task<IPagedList<SupplierAddressEntity>> GetSupplierAddresses(long supplierId, PagingInfoModel pagingInfo, CancellationToken cancellationToken)
    {
        var addresses = await this.context.SupplierAddresses
            .AsActive()
            .Include(x => x.Address)
            .ThenInclude(x => x.Country)
            .Include(x => x.Address)
            .ThenInclude(x => x.Region)
            .Include(x => x.Address)
            .ThenInclude(x => x.City)
            .Where(x => x.SupplierId == supplierId)
            .ToPagedListEx(pagingInfo, cancellationToken: cancellationToken);

        return addresses;
    }

    public async Task<IEnumerable<SupplierAddressEntity>> GetAddresses(IEnumerable<long> supplierIds, CancellationToken cancellationToken)
    {
        return await this.context.SupplierAddresses
            .AsActive()
            .Include(x => x.Address)
            .ThenInclude(x => x.Country)
            .Include(x => x.Address)
            .ThenInclude(x => x.Region)
            .Include(x => x.Address)
            .ThenInclude(x => x.City)
            .Where(x => supplierIds.Contains(x.SupplierId))
            .ToListAsync(cancellationToken);
    }
}