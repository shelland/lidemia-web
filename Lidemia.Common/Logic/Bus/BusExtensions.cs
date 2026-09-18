// Created on 17/09/2026 19:21 by Laserson

using Lidemia.Common.Logic.Bus.Consumers;
using Lidemia.Core.Extensions;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lidemia.Common.Logic.Bus;

public static class BusExtensions
{
    public static IServiceCollection RegisterBus(this IServiceCollection service, IConfiguration configuration)
    {
        var url = configuration["Integrations:Rabbit:Url"].NotNull();
        var vHost = configuration["Integrations:Rabbit:Vhost"].NotNull();
        var userName = configuration["Integrations:Rabbit:Name"].NotNull();
        var password = configuration["Integrations:Rabbit:Password"].NotNull();
        var port = ushort.Parse(configuration["Integrations:Rabbit:Port"].NotNull());

        service.AddMassTransit(busConfig =>
        {
            busConfig.AddConsumer<SupplierSignUpEventConsumer>();

            busConfig.UsingRabbitMq((ctx, rabbitConf) =>
            {
                rabbitConf.Host(url, port, vHost, localCfg =>
                {
                    localCfg.Username(userName);
                    localCfg.Password(password);
                });

                rabbitConf.UseJsonSerializer();
                rabbitConf.ReceiveEndpoint("supplier-signup", x => x.ConfigureConsumer<SupplierSignUpEventConsumer>(ctx));

                rabbitConf.ConfigureEndpoints(ctx);
            });
        });

        return service;
    }
}