
using Lidemia.Common.BusinessLogic;
using Lidemia.Common.Logic.Logging;
using Lidemia.Core.Models.Configuration;
using Lidemia.DataAccess;
using Lidemia.ServiceDefaults;
using Lidemia.Storage.Logic.Extensions;

namespace Lidemia.Storage;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

        builder.Services.Configure<ApplicationSettingsModel>(builder.Configuration.GetSection("Application"));

        builder.Services.AddControllers();
        builder.Services.RegisterLogging(builder.Configuration);
        builder.Services.RegisterBus(builder.Configuration);
        builder.Services.AddCommonBusinessLogicModule(builder.Configuration);
        builder.Services.AddDataAccessModule(builder.Configuration);

        builder.Services.RegisterStorageModule();

        var app = builder.Build();

        app.MapDefaultEndpoints();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
        }

        app.UseStaticFiles();

        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
