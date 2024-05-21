
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using StackExchange.Redis;
using Talabat.Application.Auth_Service;
using Talabat.APIs.Helpers;
using Talabat.APIs.Middlewares;
using Talabat.Core.Repositories.Contract;
using Talabat.Infrastructure;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.Extensions.DependencyInjection;
using Talabat.Infrastructure.Basket_Repository;
using Talabat.Infrastructure.Data;
using Talabat.Infrastructure._Identity;
using Microsoft.AspNetCore.Identity;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services.Contract;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Talabat.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var webApplicationBuilder = WebApplication.CreateBuilder(args);

            #region ConfigureServices
            // Add services to the DI container.
            webApplicationBuilder.Services.AddControllers().AddNewtonsoftJson(options => {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            webApplicationBuilder.Services.AddSwaggerServices();
            webApplicationBuilder.Services.AddDbContext<ApplicationDbContext>((options) =>
            {
                options.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection"));
            });
            webApplicationBuilder.Services.AddSingleton<IConnectionMultiplexer>((servicesProvider) => {
                var connection = webApplicationBuilder.Configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(connection);
            });

            webApplicationBuilder.Services.AddApplicationServices();

            webApplicationBuilder.Services.AddDbContext<ApplicationIdentityDbContext>((options) => {
                options.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("IdentityConnection"));
            });

            webApplicationBuilder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationIdentityDbContext>();

            webApplicationBuilder.Services.AddScoped<IAuthService, AuthService>();

            webApplicationBuilder.Services.AddAuthServices(webApplicationBuilder.Configuration);

            #endregion

            var app = webApplicationBuilder.Build();

            using var Scope = app.Services.CreateScope();
            var Services = Scope.ServiceProvider;
            var loggerFactory = Services.GetRequiredService<ILoggerFactory>();
            var _dbContext = Services.GetRequiredService<ApplicationDbContext>();
            var _applicationIdentityDbContext = Services.GetRequiredService<ApplicationIdentityDbContext>();
            var _userManger = Services.GetRequiredService<UserManager<ApplicationUser>>();
            //Ask CLR for object form ApplicationDbContextExplicitly
            try
            {
                await _dbContext.Database.MigrateAsync(); // Apply All Migration 
                await ApplicationContextSeed.SeedAsync(_dbContext);  //Data Seeding
                await _applicationIdentityDbContext.Database.MigrateAsync();
                await ApplicationIdentityContextSeed.SeedUsersAsync(_userManger);
            }
            catch (Exception ex)
            {
                var Logger = loggerFactory.CreateLogger("Logger");
                Logger.LogWarning(ex, "An Error Occured wile updating database");
            }


            #region Configure
            // Configure the HTTP request pipeline.

            app.UseMiddleware<ExceptionMiddleware>();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerServicesMiddlwares();
            }

            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            //app.UseRouting();
            //app.UseEndpoints(endpoints =>
            //{
            //  /// for mvc
            //	//endpoints.MapControllerRoute(
            //	//	name: "default",
            //	//	pattern: "api/{controller}/{action}/{id?}"
            //	//	);

            // /// for api
            //	endpoints.MapControllers();
            //});

            // use this middelware instead of the 2 commented in top
            app.MapControllers();
            #endregion

            app.Run();
        }

    }
}
