// Created on 01/09/2026 15:44 by Laserson

using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Repository.Abstract.Base;

namespace Lidemia.DataAccess.Repository.Abstract;

public interface IOrderRepository : IRepository<OrderEntity, long>
{
}