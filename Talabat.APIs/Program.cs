
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
using Talabat.Infrastructure._Data;
using Talabat.Infrastructure._Identity;
using Microsoft.AspNetCore.Identity;
using Talabat.Core.Entitites.Identity;
using Talabat.Core.Services.Contract;

namespace Talabat.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            var webApplicationBuilder = WebApplication.CreateBuilder(args);

            #region Configure Service method from .net 5.0 
            // Add services to the container.

            webApplicationBuilder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
           
            webApplicationBuilder.Services.AddSwaggerServices();

            webApplicationBuilder.Services.AddDbContext<StoreContext>(option =>
            {
                option.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection"));
            });

            webApplicationBuilder.Services.AddApplicationServices();

            webApplicationBuilder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
            {
                options.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("IdentityConnection"));
            });

			webApplicationBuilder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
			{
			}).AddEntityFrameworkStores<ApplicationIdentityDbContext>();

			webApplicationBuilder.Services.AddAuthServices(webApplicationBuilder.Configuration);


			webApplicationBuilder.Services.AddScoped<IConnectionMultiplexer>(serviceProvider =>
           {
            var connection = webApplicationBuilder.Configuration.GetConnectionString("Redis");
            return ConnectionMultiplexer.Connect("connection");
           });

            webApplicationBuilder.Services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));

            #endregion
            var app = webApplicationBuilder.Build();

            using var scope = app.Services.CreateScope();

            var services = scope.ServiceProvider;

            var _dbContext = services.GetRequiredService<StoreContext>();
            var _identityDbContext = services.GetRequiredService<ApplicationIdentityDbContext>();

            var loggerFactory = services.GetRequiredService<ILoggerFactory>();

            try
            {
                await _dbContext.Database.MigrateAsync(); //Update-Database
                await StoreContextSeed.SeedAsync(_dbContext); //Data Seeding

                await _identityDbContext.Database.MigrateAsync();
				var _userManger = services.GetRequiredService<UserManager<ApplicationUser>>();
				await ApplicationIdentityContextSeed.SeedUsersAsync(_userManger);
			}
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "an error has been occured during apply the migration");
            }

            #region Configure Kestrel Middleware
            // Configure the HTTP request pipeline.
            //app.UseMiddleware<ExceptionMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerServicesMiddlwares();
            }
            //app.UseStatusCodePagesWithReExecute("/errors/{0}");
            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.MapControllers();
            
            #endregion
            app.Run();
        }
    }
}
