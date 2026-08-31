// Created on 01/09/2026 14:39 by Laserson

using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Repository.Abstract.Base;

namespace Lidemia.DataAccess.Repository.Abstract;

public interface IProductRepository : IRepository<ProductEntity, long>
{
}