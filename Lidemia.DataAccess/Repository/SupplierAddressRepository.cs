// Created on 18/09/2026 19:24 by Laserson

using FluentResults;
using Lidemia.Core.Models.Service;
using Lidemia.DataAccess.Context;
using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Extensions;
using Lidemia.DataAccess.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.DataAccess.Repository;

[ServiceDescriptor<ISupplierAddressRepository>(ServiceLifetime.Scoped)]
public class SupplierAddressRepository : ISupplierAddressRepository
{
    private readonly LidemiaDbContext context;

    public SupplierAddressRepository(LidemiaDbContext context)
    {
        this.context = context;
    }

    public Task<SupplierAddressEntity?> GetById(Guid key, CancellationToken cancellationToken)
    {
        return this.context.SuppliersAddresses
            .Include(x => x.Address)
            .AsActive()
            .FirstOrDefaultAsync(x => x.Id == key, cancellationToken: cancellationToken);
    }

    public Task Delete(Guid key, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<Guid>> Save(SaveSupplierAddressModel model, CancellationToken cancellationToken)
    {
        Guid id;

        if (model.Id != null)
        {
            id = model.Id.Value;

            await this.context.SuppliersAddresses
                .Where(x => x.Id == model.Id)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(e => e.Comment, model.Comment)
                    .SetProperty(e => e.Type, model.Type)
                    .SetProperty(e => e.Address.CountryId, model.CountryId)
                    .SetProperty(e => e.Address.RegionId, model.RegionId)
                    .SetProperty(e => e.Address.CityId, model.CityId)
                    .SetProperty(e => e.Address.StreetName, model.StreetName)
                    .SetProperty(e => e.Address.HouseAdd1, model.HouseNumber)
                    .SetProperty(e => e.Address.FlatNumber, model.FlatNumber), cancellationToken: cancellationToken);
        }
        else
        {
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
                }
            };

            this.context.SuppliersAddresses.Add(entity);
            id = entity.Id;
        }

        await this.context.SaveChangesAsync(cancellationToken);
        return Result.Ok(id);
    }
}