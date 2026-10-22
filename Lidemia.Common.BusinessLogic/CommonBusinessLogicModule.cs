// Created on 02/09/2026 20:11 by Laserson

using Lidemia.Core.Models.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;

namespace Lidemia.Common.BusinessLogic;

public static class CommonBusinessLogicModule
{
    public static IServiceCollection AddCommonBusinessLogicModule(this IServiceCollection service, IConfiguration configuration)
    {
        service.Scan(x => x.FromAssemblyOf<ICommonBusinessLogicModule>().AddClasses().UsingAttributes());

        var section = configuration.GetSection("LocalServices:Minio");
        var settings = section.Get<MinioSettings>() ?? new MinioSettings();

        service.Configure<MinioSettings>(section);

        service.AddSingleton<IMinioClient>(_ => new MinioClient()
            .WithEndpoint(settings.BaseUrl)
            .WithCredentials(settings.AccessKey, settings.SecretKey)
            .WithSSL(settings.UseSsl)
            .Build());

        return service;
    }
}

public interface ICommonBusinessLogicModule;