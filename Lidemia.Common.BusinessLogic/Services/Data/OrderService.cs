// Created on 03/09/2026 18:48 by Laserson

using Lidemia.Common.BusinessLogic.Services.App.Abstract;
using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.DataAccess.Repository.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.Common.BusinessLogic.Services.Data;

[ServiceDescriptor<IOrderService>(ServiceLifetime.Scoped)]
public class OrderService : IOrderService
{
    private readonly IOrderRepository orderRepository;
    private readonly IOrderNumberGenerator orderNumberGenerator;

    public OrderService(IOrderRepository orderRepository, IOrderNumberGenerator orderNumberGenerator)
    {
        this.orderRepository = orderRepository;
        this.orderNumberGenerator = orderNumberGenerator;
    }
}