// Created on 20/10/2026 18:38 by Laserson

using Lidemia.Core.Extensions;
using Lidemia.Storage.Logic.Consumers;
using MassTransit;

namespace Lidemia.Storage.Logic.Extensions;

public static class BusExtensions
{
    public static IServiceCollection RegisterBus(this IServiceCollection services, IConfiguration configuration)
    {
        var url = configuration["LocalServices:Rabbit:Url"].NotNull();
        var vHost = configuration["LocalServices:Rabbit:Vhost"].NotNull();
        var userName = configuration["LocalServices:Rabbit:Name"].NotNull();
        var password = configuration["LocalServices:Rabbit:Password"].NotNull();
        var port = ushort.Parse(configuration["LocalServices:Rabbit:Port"].NotNull());

        services.AddMassTransit(busConfig =>
        {
            busConfig.AddConsumer<ProductPhotosUploadedEventConsumer>();

            busConfig.UsingRabbitMq((ctx, rabbitConf) =>
            {
                rabbitConf.Host(url, port, vHost, localCfg =>
                {
                    localCfg.Username(userName);
                    localCfg.Password(password);
                });

                rabbitConf.UseJsonSerializer();
                rabbitConf.ReceiveEndpoint("product-photos-uploaded", x => x.ConfigureConsumer<ProductPhotosUploadedEventConsumer>(ctx));

                rabbitConf.ConfigureEndpoints(ctx);
            });
        });

        return services;
    }
}