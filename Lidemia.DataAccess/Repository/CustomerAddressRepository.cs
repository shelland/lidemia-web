// Created on 18/09/2026 19:25 by Laserson

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

[ServiceDescriptor<ICustomerAddressRepository>(ServiceLifetime.Scoped)]
public class CustomerAddressRepository : ICustomerAddressRepository
{
    private readonly LidemiaDbContext context;

    public CustomerAddressRepository(LidemiaDbContext context)
    {
        this.context = context;
    }

    public async Task<IPagedList<CustomerAddressEntity>> GetCustomerAddresses(long customerId, PagingInfoModel pagingInfo,
        CancellationToken cancellationToken)
    {
        return await this.context.CustomerAddresses
            .AsActive()
            .Include(x => x.Address)
            .ThenInclude(x => x.Country)
            .Include(x => x.Address)
            .ThenInclude(x => x.Region)
            .Include(x => x.Address)
            .ThenInclude(x => x.City)
            .Where(x => x.CustomerId == customerId)
            .ToPagedListEx(pagingInfo, cancellationToken: cancellationToken);
    }

    public async Task<Result<long>> Save(SaveCustomerAddressModel model, CancellationToken cancellationToken)
    {
        if (model.Id != null)
        {
            var customerAddress = await this.context.CustomerAddresses
                .AsTracking()
                .Include(x => x.Address)
                .FirstOrDefaultAsync(x => x.Id == model.Id && x.CustomerId == model.CustomerId, cancellationToken: cancellationToken);

            if (customerAddress == null)
            {
                return Result.Fail(CommonErrors.NotFoundError);
            }

            customerAddress.Comment = model.Comment;
            customerAddress.Address.CountryId = model.CountryId;
            customerAddress.Address.RegionId = model.RegionId;
            customerAddress.Address.CityId = model.CityId;
            customerAddress.Address.StreetName = model.StreetName;
            customerAddress.Address.HouseAdd1 = model.HouseNumber;
            customerAddress.Address.FlatNumber = model.FlatNumber;

            await this.context.SaveChangesAsync(cancellationToken);

            return Result.Ok(customerAddress.Id);
        }

        var entity = new CustomerAddressEntity
        {
            CustomerId = model.CustomerId,
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

        this.context.CustomerAddresses.Add(entity);
        await this.context.SaveChangesAsync(cancellationToken);

        return Result.Ok(entity.Id);
    }

    public Task<CustomerAddressEntity?> GetById(long key, CancellationToken cancellationToken)
    {
        return this.context.CustomerAddresses
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
        return this.context.CustomerAddresses
            .AsActive()
            .Where(x => x.Id == key)
            .ExecuteUpdateAsync(x => x.SetProperty(e => e.IsActive, false), cancellationToken);
    }
}