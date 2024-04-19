
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.APIs.Helpers;
using Talabat.APIs.Middlewares;
using Talabat.Core.Repositories.Contract;
using Talabat.Infrastructure;
using Talabat.Infrastructure.Data;

namespace Talabat.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            var webApplicationBuilder = WebApplication.CreateBuilder(args);

            #region Configure Service method from dot net 5 
            // Add services to the container.

            webApplicationBuilder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
           
            webApplicationBuilder.Services.AddSwaggerServices();

            webApplicationBuilder.Services.AddDbContext<StoreContext>(option =>
            {
                option.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection"));
            });

            webApplicationBuilder.Services.AddApplicationServices();
            #endregion
            var app = webApplicationBuilder.Build();

            using var scope = app.Services.CreateScope();

            var services = scope.ServiceProvider;

            var _dbContext = services.GetRequiredService<StoreContext>();


            var loggerFactory = services.GetRequiredService<ILoggerFactory>();

            try
            {
                await _dbContext.Database.MigrateAsync(); //Update-Database
                await StoreContextSeed.SeedAsync(_dbContext); //Data Seeding
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);

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
