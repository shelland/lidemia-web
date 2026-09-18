// Created on 18/09/2026 19:24 by Laserson

using Lidemia.DataAccess.Repository.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.DataAccess.Repository;

[ServiceDescriptor<ISupplierAddressRepository>(ServiceLifetime.Scoped)]
public class SupplierAddressRepository : ISupplierAddressRepository
{
}