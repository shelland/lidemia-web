using Lidemia.Common;
using Lidemia.Common.BusinessLogic;
using Lidemia.DataAccess;
using Lidemia.Search;
using Lidemia.ServiceDefaults;

namespace Lidemia.Admin;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        builder.Services.AddAdminModule();
        builder.Services.AddDataAccessModule(builder.Configuration);
        builder.Services.AddCommonBusinessLogicModule(builder.Configuration);
        builder.Services.AddCommonModule();
        builder.Services.AddSearchModule(builder.Configuration);

        var app = builder.Build();

        app.MapDefaultEndpoints();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthorization();

        app.MapStaticAssets();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}")
            .WithStaticAssets();

        app.Run();
    }
}
