// Created on 11/10/2026 15:31 by Laserson

using FluentResults;
using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Common.Mapping;
using Lidemia.Core.Models.Domain;
using Lidemia.Core.Models.Misc;
using Lidemia.Core.Models.Service;
using Lidemia.DataAccess.Repository.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.Common.BusinessLogic.Services.Data;

[ServiceDescriptor<ISupplierAddressService>(ServiceLifetime.Scoped)]
public class SupplierAddressService : BaseDataService, ISupplierAddressService
{
    private readonly ISupplierAddressRepository supplierAddressRepository;

    public SupplierAddressService(ISupplierAddressRepository supplierAddressRepository)
    {
        this.supplierAddressRepository = supplierAddressRepository;
    }

    public Task<BasePagedListModel<SupplierAddressModel>> GetAddresses()
    {
        throw new NotImplementedException();
    }

    public async Task<SupplierAddressModel?> GetById(Guid id, CancellationToken cancellationToken)
    {
        var address = await this.supplierAddressRepository.GetById(id, cancellationToken);
        return address?.ToModel();
    }

    public async Task<Result<Guid>> Save(long supplierId, SaveSupplierAddressRequestDto request, CancellationToken cancellationToken)
    {
        if (request.Id != null)
        {
            var supplierAddress = await this.supplierAddressRepository.GetById(request.Id.Value, cancellationToken);

            if (supplierAddress?.SupplierId != supplierId)
            {
                ThrowNotAuthorized();
            }
        }

        var result = await this.supplierAddressRepository.Save(new SaveSupplierAddressModel(
            SupplierId: supplierId,
            Id: request.Id,
            CountryId: request.CountryId,
            RegionId: request.RegionId,
            CityId: request.CityId,
            StreetName: request.StreetName,
            HouseNumber: request.HouseNumber,
            FlatNumber: request.FlatNumber,
            Comment: request.Comment,
            Type: request.Type
        ), cancellationToken);

        return result;
    }
}