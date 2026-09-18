// Created on 18/09/2026 19:24 by Laserson

using Lidemia.DataAccess.Entities;

namespace Lidemia.DataAccess.Repository.Abstract;

public interface ICustomerAddressRepository
{
    Task<IReadOnlyList<AddressEntity>> GetCustomerAddressed(long customerId, CancellationToken cancellationToken);
}